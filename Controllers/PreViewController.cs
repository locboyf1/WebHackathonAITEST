using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebHackathon.Models;
using System.Linq;
using WebHackathon.Utilities;


namespace WebHackathon.Controllers
{
    public class BookInfoDto
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
    public class PreViewController : Controller
    {
        private readonly DbHackathonContext _context;

        public PreViewController(DbHackathonContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/preview";  // Lưu URL hiện tại để redirect sau khi login
                return Redirect("/login");
            }
            var books = await _context.TbBooks
                .Include(b => b.Author)
                .Include(b => b.Category)
                .OrderBy(b => b.BookId)
                .Select(b => new BookInfoDto
                {
                    Title = b.Title,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                    CategoryName = b.Category != null ? b.Category.Title : "No Category",
                    Description = b.Description
                })
                .ToListAsync();

            return View(books);
        }
    }
}