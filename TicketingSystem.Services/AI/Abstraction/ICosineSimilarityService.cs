namespace TicketingSystem.Services.AI.Abstraction
{
    public interface ICosineSimilarityService
    {
        double Compute(float[] a, float[] b);
    }
}
