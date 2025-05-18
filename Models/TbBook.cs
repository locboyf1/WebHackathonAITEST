using System;
using System.Collections.Generic;

namespace WebHackathon.Models;

public partial class TbBook
{
    public int BookId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? BookPdf { get; set; }

    public int? Star { get; set; }

    public int? CategoryId { get; set; }

    public int? AuthorId { get; set; }

    public int? PublisherId { get; set; }

    public int? Price { get; set; }
    public string? Image {  get; set; }
    public virtual TbAuthor? Author { get; set; }
    public virtual TbCategory? Category { get; set; }

    public virtual TbPublisher? Publisher { get; set; }

    public virtual ICollection<TbBorrow> TbBorrows { get; set; } = new List<TbBorrow>();

    public virtual ICollection<TbCart> TbCarts { get; set; } = new List<TbCart>();

    public virtual ICollection<TbChapterBook> TbChapterBooks { get; set; } = new List<TbChapterBook>();

    public virtual ICollection<TbDownloaded> TbDownloadeds { get; set; } = new List<TbDownloaded>();

    public virtual ICollection<TbReview> TbReviews { get; set; } = new List<TbReview>();

    public virtual ICollection<TbSavedBook> TbSavedBooks { get; set; } = new List<TbSavedBook>();
}
