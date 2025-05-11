using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class DocumentChunk
{
    public int Id { get; set; }

    public string? ChunkText { get; set; }

    public string? Embedding { get; set; }
}
