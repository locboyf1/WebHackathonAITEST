using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbCart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? TotalPrice { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TbUser? User { get; set; }
}
