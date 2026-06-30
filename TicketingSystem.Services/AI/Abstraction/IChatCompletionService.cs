namespace TicketingSystem.Services.AI.Abstraction
{
    public interface IChatCompletionService
    {
        Task<string> CompleteAsync(string systemPrompt, string userMessage, CancellationToken cancellationToken = default);
    }
}
