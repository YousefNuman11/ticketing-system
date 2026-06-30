namespace TicketingSystem.Services.AI
{
    public enum ChatIntent
    {
        TicketLookup,
        Analytics
    }

    public interface IChatIntentClassifier
    {
        Task<ChatIntent> ClassifyAsync(string question, CancellationToken cancellationToken = default);
    }
}