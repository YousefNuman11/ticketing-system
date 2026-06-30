using MediatR;
using System.Text.Json;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.AI;
using TicketingSystem.Services.AI.Abstraction;
using TicketingSystem.Services.AI.Prompts;
using TicketingSystem.Services.Features.ChatMediator.Contract;

namespace TicketingSystem.Services.Features.ChatMediator.Queries
{
    public class AskChatbotQueryHandler
        : IRequestHandler<AskChatbotQuery, ChatResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmbeddingService _embeddingService;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ICosineSimilarityService _similarityService;
        private readonly IChatIntentClassifier _intentClassifier;
        private readonly IRagPromptBuilder _promptBuilder;

        private const double SimilarityThreshold = 0.75;
        private const int MaxResults = 3;

        private const string RefusalMessage =
            "This is not in the system, I can't help you with that.";

        private const string AnalyticsPermissionDeniedMessage =
            "I can't help with that — analytics questions are only available to managers.";

        public AskChatbotQueryHandler(
            IUnitOfWork unitOfWork,
            IEmbeddingService embeddingService,
            IChatCompletionService chatCompletionService,
            ICosineSimilarityService similarityService,
            IChatIntentClassifier intentClassifier,
            IRagPromptBuilder promptBuilder)
        {
            _unitOfWork = unitOfWork;
            _embeddingService = embeddingService;
            _chatCompletionService = chatCompletionService;
            _similarityService = similarityService;
            _intentClassifier = intentClassifier;
            _promptBuilder = promptBuilder;
        }

        public async Task<ChatResponseDto> Handle(
            AskChatbotQuery request, CancellationToken cancellationToken)
        {
            // 0. Intent gating — analytics questions are Manager-only.
            // This check happens BEFORE any retrieval or OpenAI embedding/completion
            // call for the RAG path, so a non-Manager asking an analytics question
            // never triggers that cost. (Classification itself is one OpenAI call.)
            var intent = await _intentClassifier.ClassifyAsync(request.Question, cancellationToken);

            if (intent == ChatIntent.Analytics)
            {
                if (request.Role != UserRole.Manager)
                {
                    return new ChatResponseDto
                    {
                        Answer = AnalyticsPermissionDeniedMessage,
                        SourceTicketIds = new List<Guid>()
                    };
                }

                // Analytics path for Managers — not implemented yet (deferred).
                return new ChatResponseDto
                {
                    Answer = "Analytics answers aren't available yet — coming soon.",
                    SourceTicketIds = new List<Guid>()
                };
            }

            // 1. RAG path — available to everyone (Client, Employee, Manager)
            var questionEmbedding = await _embeddingService.EmbedAsync(request.Question, cancellationToken);

            var resolvedTickets = await _unitOfWork.Tickets.GetAllAsync();
            var candidates = resolvedTickets
                .Where(t => t.Status == TicketStatus.Resolved && t.EmbeddingJson != null)
                .ToList();

            // 2. Score each candidate by cosine similarity
            var scored = new List<(Ticket Ticket, double Score)>();
            foreach (var ticket in candidates)
            {
                var embedding = JsonSerializer.Deserialize<float[]>(ticket.EmbeddingJson!)!;
                var score = _similarityService.Compute(questionEmbedding, embedding);
                scored.Add((ticket, score));
            }

            var topMatches = scored
                .Where(s => s.Score >= SimilarityThreshold)
                .OrderByDescending(s => s.Score)
                .Take(MaxResults)
                .ToList();

            // 3. No grounding found -> refuse immediately, no chat completion call needed
            if (topMatches.Count == 0)
            {
                return new ChatResponseDto
                {
                    Answer = RefusalMessage,
                    SourceTicketIds = new List<Guid>()
                };
            }

            // 4. Build the strictly-grounded prompt from the matched tickets
            var systemPrompt = _promptBuilder.BuildSystemPrompt(
                topMatches.Select(m => m.Ticket), RefusalMessage);

            var answer = await _chatCompletionService.CompleteAsync(
                systemPrompt, request.Question, cancellationToken);

            return new ChatResponseDto
            {
                Answer = answer,
                SourceTicketIds = topMatches.Select(m => m.Ticket.Id).ToList()
            };
        }
    }
}