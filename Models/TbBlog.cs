using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbBlog
{
    public int BlogId { get; set; }

    public int? BlogCategoryId { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Detail { get; set; }

    public int? TagBlogId { get; set; }

    public string? Image { get; set; }

    public DateTime? Date { get; set; }

    public int? LikeCount { get; set; }

    public int? ViewCount { get; set; }

    public bool IsShow { get; set; }

    public virtual TbBlogCategory? BlogCategory { get; set; }

    public virtual ICollection<TbComment> TbComments { get; set; } = new List<TbComment>();

    public virtual ICollection<TbTagBlog> TbTagBlogs { get; set; } = new List<TbTagBlog>();

    public virtual TbUser? User { get; set; }
}
