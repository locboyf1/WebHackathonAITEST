using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Models;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagBlogsController : Controller
    {
        private readonly DbHackathonContext _context;

        public TagBlogsController(DbHackathonContext context)
        {
            _context = context;
        }

        // GET: Admin/TagBlogs
        public IActionResult Index(int id)
        {
            var blog = _context.TbBlogs.FirstOrDefault(i => i.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.blog = blog;
            List<TbTagBlog> tag = _context.TbTagBlogs.Where(t => t.BlogId == id).ToList();
            return View(tag);
        }

        // POST: Admin/TagBlogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(int idblog, string tag)
        {
            TbTagBlog tbTagBlog = new TbTagBlog
            {
                Tag = tag,
                BlogId = idblog
            };

            _context.TbTagBlogs.Add(tbTagBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = idblog });

        }

        // POST: Admin/TagBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var tbTagBlog = await _context.TbTagBlogs.FindAsync(id);

            if (tbTagBlog == null)
            {
                return NotFound();
            }

            int idblog = tbTagBlog.BlogId;
            _context.TbTagBlogs.Remove(tbTagBlog);

            await _context.SaveChangesAsync();
            return Redirect($"/Admin/TagBlogs/Index/{idblog}");
        }

        private bool TbTagBlogExists(int id)
        {
            return _context.TbTagBlogs.Any(e => e.TagBlogId == id);
        }
    }
}
