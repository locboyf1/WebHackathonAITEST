using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class EmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "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";

    public EmbeddingService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<List<float>> GetEmbeddingAsync(string input)
    {
        var requestData = new
        {
            input = input,
            model = "text-embedding-3-small"
        };

        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("embeddings", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Embedding API error: {responseBody}");

        using var doc = JsonDocument.Parse(responseBody);
        var embeddingJson = doc.RootElement.GetProperty("data")[0].GetProperty("embedding");
        var embedding = embeddingJson.EnumerateArray().Select(x => x.GetSingle()).ToList();

        return embedding;
    }
}
