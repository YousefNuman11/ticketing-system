using TicketingSystem.Services.AI.Abstraction;
using TicketingSystem.Services.AI.Prompts;

namespace TicketingSystem.Services.AI
{
    // Uses the chat completion model itself to classify intent, rather than
    // brittle keyword matching. Asks for a single-word answer and parses it
    // defensively, since LLM output isn't guaranteed to be perfectly clean.
    public class LlmChatIntentClassifier : IChatIntentClassifier
    {
        private readonly IChatCompletionService _chatCompletionService;

        public LlmChatIntentClassifier(IChatCompletionService chatCompletionService)
        {
            _chatCompletionService = chatCompletionService;
        }

        public async Task<ChatIntent> ClassifyAsync(string question, CancellationToken cancellationToken = default)
        {
            var response = await _chatCompletionService.CompleteAsync(
                ChatIntentPrompts.ClassifySystemPrompt, question, cancellationToken);

            var normalized = response.Trim().ToUpperInvariant();

            // Defensive parsing: the model should return exactly one of these two
            // tokens, but LLM output is never 100% guaranteed to be clean, so we
            // check via Contains rather than exact equality, and default safely
            // to TicketLookup (the less-privileged path) if parsing is ambiguous.
            // This matters for security: a failed/ambiguous classification must
            // never accidentally grant access to the Manager-only analytics path.
            if (normalized.Contains("ANALYTICS"))
                return ChatIntent.Analytics;

            return ChatIntent.TicketLookup;
        }
    }
}