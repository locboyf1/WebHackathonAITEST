using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbMenu
{
    public int MenuId { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? Url { get; set; }

    public int? Position { get; set; }

    public int? ParentId { get; set; }

    public bool IsShow { get; set; }
}
