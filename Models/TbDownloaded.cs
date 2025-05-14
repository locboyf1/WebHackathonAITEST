using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbDownloaded
{
    public int DownloadedId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateTime? Time { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TbUser? User { get; set; }
}
