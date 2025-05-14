using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbSavedBook
{
    public int SavedBookId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public virtual TbBook? Book { get; set; }
}
