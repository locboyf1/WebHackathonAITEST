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
        public int? Price { get; set; }
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
                    Price = b.Price ?? 0
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