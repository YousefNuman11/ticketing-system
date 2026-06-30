namespace TicketingSystem.Services.AI.Abstraction
{
    public interface IEmbeddingService
    {
        Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default);
    }
}
