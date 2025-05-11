using System;
using System.Text.Json;
using WebHackathon.Models;


public class DocumentChunkService
{
    private readonly DbHackathonContext _context;

    public DocumentChunkService(DbHackathonContext context)
    {
        _context = context;
    }

    public async Task SaveChunkAsync(string chunkText, List<float> embedding)
    {
        var json = JsonSerializer.Serialize(embedding);
        var chunk = new DocumentChunk
        {
            ChunkText = chunkText,
            Embedding = json
        };

        _context.DocumentChunks.Add(chunk);
        await _context.SaveChangesAsync();
    }
}
