using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebHackathon.Models;
using WebHackathon.Services;

namespace WebHackathon.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly DbHackathonContext _context;
        private readonly CohereEmbeddingService _embeddingService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChatbotController(DbHackathonContext context, CohereEmbeddingService embeddingService, IConfiguration config)
        {
            _context = context;
            _embeddingService = embeddingService;
            _configuration = config;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string userQuestion)
        {
            if (string.IsNullOrWhiteSpace(userQuestion))
            {
                ViewBag.Answer = "Bạn chưa nhập câu hỏi.";
                return View();
            }

            var bestChunk = await FindBestMatchingChunkAsync(userQuestion);
            var answer = await GetAnswerFromCohereAsync(userQuestion, bestChunk);

            ViewBag.Answer = answer;
            return View();
        }

        private async Task<string> FindBestMatchingChunkAsync(string userQuestion)
        {
            var questionEmbedding = await _embeddingService.GetEmbeddingAsync(userQuestion);
            var allChunks = await _context.DocumentChunks.ToListAsync();

            float bestScore = -1f;
            string bestChunk = "";

            foreach (var chunk in allChunks)
            {
                var embedding = JsonSerializer.Deserialize<List<float>>(chunk.Embedding);
                float score = SimilarityService.CosineSimilarity(questionEmbedding, embedding);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestChunk = chunk.ChunkText;
                }
            }

            return bestChunk;
        }

        private async Task<string> GetAnswerFromCohereAsync(string userQuestion, string contextChunk)
        {
            var apiKey = _configuration["Cohere:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new
            {
                model = "command-r-plus",
                prompt = $"Tài liệu:\n{contextChunk}\n\nCâu hỏi: {userQuestion}",
                max_tokens = 300,
                temperature = 0.5
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.cohere.ai/v1/generate", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Lỗi API Cohere: {response.StatusCode} - {responseBody}";
            }

            using var doc = JsonDocument.Parse(responseBody);
            var generations = doc.RootElement.GetProperty("generations");
            var text = generations[0].GetProperty("text").GetString();

            return text ?? "Không có nội dung trả lời.";
        }

    }
}