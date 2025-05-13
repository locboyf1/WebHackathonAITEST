using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class CohereEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CohereEmbeddingService(IConfiguration configuration)
    {
        _apiKey = configuration["Cohere:ApiKey"];
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.cohere.ai")
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<List<float>> GetEmbeddingAsync(string text)
    {
        var payload = new
        {
            model = "embed-english-v3.0",
            texts = new[] { text },
            input_type = "search_document" // 👈 Bắt buộc với v3.0
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/v1/embed", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Cohere API error: {responseBody}");

        using var doc = JsonDocument.Parse(responseBody);
        var embeddingJson = doc.RootElement.GetProperty("embeddings")[0];
        var embedding = embeddingJson.EnumerateArray().Select(e => e.GetSingle()).ToList();

        return embedding;
    }

}
