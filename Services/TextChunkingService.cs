public static class TextChunkingService
{
    public static List<string> SplitText(string text, int maxTokens)
    {
        var chunks = new List<string>();
        var words = text.Split(' ');
        var buffer = new List<string>();
        foreach (var word in words)
        {
            buffer.Add(word);
            if (buffer.Count >= maxTokens)
            {
                chunks.Add(string.Join(" ", buffer));
                buffer.Clear();
            }
        }

        if (buffer.Count > 0)
            chunks.Add(string.Join(" ", buffer));

        return chunks;
    }
}
