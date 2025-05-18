using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using WebHackathon.Models;
using WebHackathon.Utilities;
using Microsoft.EntityFrameworkCore;

namespace WebHackathon.Controllers
{
    public class BlogsController : Controller
    {
        private readonly DbHackathonContext _context;
        public BlogsController(DbHackathonContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int category = 0)
        {
            const int pageSize = 10;
            ViewBag.page = page;
            ViewBag.Category = category;
            ViewBag.page_num = (int)Math.Ceiling((double)_context.TbBlogs.Count() / pageSize);

            var blogs = _context.TbBlogs.Include(i => i.BlogCategory).Include(i => i.TbComments).Include(i => i.User).Include(i => i.TbTagBlogs).Skip((page - 1) * pageSize).Take(pageSize).AsQueryable();
            if (category != 0) { 
                blogs.Where(i => i.BlogCategoryId == category);
            }
            return View(blogs.ToList()); 
        }


        public IActionResult Detail(int id)
        {
            var blog = _context.TbBlogs.Include(i => i.BlogCategory).Include(i => i.TbComments).Include(i => i.User).Include(i => i.TbTagBlogs).FirstOrDefault(i => i.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }
    }
}
