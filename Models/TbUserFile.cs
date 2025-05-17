using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbUserFile
{
    public int UserFileId { get; set; }

    public int? UserId { get; set; }

    public string? Path { get; set; }

    public string? Image { get; set; }

    public bool IsAccept { get; set; }

    public virtual TbUser? User { get; set; }
}
