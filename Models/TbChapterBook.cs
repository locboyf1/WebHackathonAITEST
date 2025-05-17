using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbChapterBook
{
    public int ChapterBookId { get; set; }

    public int? BookId { get; set; }

    public int? Position { get; set; }

    public string? Subject { get; set; }

    public string? Content { get; set; }

    public virtual TbBook? Book { get; set; }
}
