namespace WebHackathon.Services
{
    public class SimilarityService
    {
        public static float CosineSimilarity(List<float> vectorA, List<float> vectorB)
        {
            if (vectorA.Count != vectorB.Count)
                throw new ArgumentException("Vectors must be the same length");

            float dot = 0f;
            float magA = 0f;
            float magB = 0f;

            for (int i = 0; i < vectorA.Count; i++)
            {
                dot += vectorA[i] * vectorB[i];
                magA += vectorA[i] * vectorA[i];
                magB += vectorB[i] * vectorB[i];
            }

            return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
        }
    }


}
