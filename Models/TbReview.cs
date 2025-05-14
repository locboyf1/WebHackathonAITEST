using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbReview
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? Star { get; set; }

    public string? Review { get; set; }

    public DateTime? Date { get; set; }

    public virtual TbBook? Book { get; set; }
}
