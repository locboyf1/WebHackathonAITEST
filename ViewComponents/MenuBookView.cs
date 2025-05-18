using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Models;

namespace WebHackathon.ViewComponents
{
    public class MenuBookView : ViewComponent
    {
        private readonly DbHackathonContext _context;

        public MenuBookView(DbHackathonContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var books = await _context.TbBooks
                .Include(b => b.Author)
                .Include(b => b.Category)
                .OrderBy(b => b.BookId)
                .Take(8)
                .Select(b => new BookInfoDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                    CategoryName = b.Category != null ? b.Category.Title : "No Category",
                    Description = b.Description,
                    Score = b.Score.ToString(),
                    Image = b.Image != null ? b.Image : "/images/no-image.png"
                })
                .ToListAsync();

            return View("Default", books);
        }
    }

    public class BookInfoDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string? Score { get; set; }
        public string? Image { get; set; }
        
    }
}