using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbPublisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbBook> TbBooks { get; set; } = new List<TbBook>();
}
