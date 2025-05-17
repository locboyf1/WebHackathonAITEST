using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbContact
{
    public int ContactId { get; set; }

    public string? Title { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Detail { get; set; }

    public DateTime? Time { get; set; }

    public bool IsResponded { get; set; }
}
