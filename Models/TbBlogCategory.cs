using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbBlogCategory
{
    public int BlogCategoryId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();
}
