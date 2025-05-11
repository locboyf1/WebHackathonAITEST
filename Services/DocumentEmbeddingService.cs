
public class DocumentEmbeddingService
{
    private readonly EmbeddingService _embeddingService;
    private readonly DocumentChunkService _chunkService;

    public DocumentEmbeddingService(EmbeddingService embeddingService, DocumentChunkService chunkService)
    {
        _embeddingService = embeddingService;
        _chunkService = chunkService;
    }

    public async Task LoadAndSaveAsync(string documentText)
    {
        var chunks = SplitIntoChunks(documentText, 500);

        foreach (var chunk in chunks)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(chunk);
            await _chunkService.SaveChunkAsync(chunk, embedding);
        }
    }

    private List<string> SplitIntoChunks(string text, int chunkSize)
    {
        var words = text.Split(' ');
        var chunks = new List<string>();

        for (int i = 0; i < words.Length; i += chunkSize)
        {
            var chunk = string.Join(" ", words.Skip(i).Take(chunkSize));
            chunks.Add(chunk);
        }

        return chunks;
    }
}
