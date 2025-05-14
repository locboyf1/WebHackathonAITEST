using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbTagBlog
{
    public int TagBlogId { get; set; }

    public int BlogId { get; set; }

    public string? Tag { get; set; }

    public virtual TbBlog Blog { get; set; } = null!;
}
