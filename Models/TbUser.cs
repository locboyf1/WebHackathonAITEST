using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbUser
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public string? Name { get; set; }

    public int? Balance { get; set; }

    public string? Avatar { get; set; }

    public bool IsLock { get; set; }

    public virtual TbRole? Role { get; set; }

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();

    public virtual ICollection<TbCart> TbCarts { get; set; } = new List<TbCart>();

    public virtual ICollection<TbDownloaded> TbDownloadeds { get; set; } = new List<TbDownloaded>();
}
