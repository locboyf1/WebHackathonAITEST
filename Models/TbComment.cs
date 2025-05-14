using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbComment
{
    public int CommentId { get; set; }

    public int? BlogId { get; set; }

    public int? UserId { get; set; }

    public string? Comment { get; set; }

    public DateTime? Date { get; set; }

    public virtual TbBlog? Blog { get; set; }
}
