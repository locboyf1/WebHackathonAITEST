using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbRole
{
    public int RoleId { get; set; }

    public string? Role { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbUser> TbUsers { get; set; } = new List<TbUser>();
}
