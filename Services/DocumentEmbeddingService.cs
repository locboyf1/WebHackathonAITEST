public class DocumentEmbeddingService
{
    private readonly CohereEmbeddingService _embeddingService;
    private readonly DocumentChunkService _chunkService;

    public DocumentEmbeddingService(CohereEmbeddingService embeddingService, DocumentChunkService chunkService)
    {
        _embeddingService = embeddingService;
        _chunkService = chunkService;
    }

    public async Task LoadAndSaveFromTextAsync(string rawText)
    {
        var chunks = TextChunkingService.SplitText(rawText, 500); foreach (var chunk in chunks)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(chunk);
            await _chunkService.SaveChunkAsync(chunk, embedding);
        }
    }

}
