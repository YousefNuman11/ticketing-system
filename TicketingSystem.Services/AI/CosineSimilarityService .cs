using TicketingSystem.Services.AI.Abstraction;

namespace TicketingSystem.Services.AI
{
    public class CosineSimilarityService : ICosineSimilarityService
    {
        public double Compute(float[] a, float[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vector must be the same length.");

            double dot = 0, magA = 0, magB = 0;

            for (var i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * b[i];
                magB += a[i] * b[i];
            }

            if (magA == 0 || magB == 0)
                return 0;

            return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));

        }
    }
}
