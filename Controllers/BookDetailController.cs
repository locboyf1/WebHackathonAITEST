using Microsoft.AspNetCore.Mvc;
using WebHackathon.Models;
using WebHackathon.Utilities;
using Microsoft.EntityFrameworkCore;

namespace WebHackathon.Controllers
{
    public class BookDetail
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int? Score { get; set; }
        public string? Image { get; set; }
    }
    public class BookDetailController : Controller
    {
        private readonly DbHackathonContext _context;
        public BookDetailController(DbHackathonContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var book = await _context.TbBooks
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.BookId == id)
                .Select(b => new BookDetail
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                    CategoryName = b.Category != null ? b.Category.Title : "No Category",
                    Description = b.Description,
                    Publisher = b.Publisher != null ? b.Publisher.Name : "Unknown Publisher",
                    Score = b.Score ?? 0,
                    Image = b.Image != null ? b.Image : "/images/no-image.png"
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}