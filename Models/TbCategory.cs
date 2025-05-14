using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbCategory
{
    public int CategoryId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbBook> TbBooks { get; set; } = new List<TbBook>();
}
