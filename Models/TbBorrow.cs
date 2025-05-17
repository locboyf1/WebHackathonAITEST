using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbBorrow
{
    public int BorrowId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TbUser? User { get; set; }
}
