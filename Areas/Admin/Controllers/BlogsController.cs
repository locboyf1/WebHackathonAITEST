using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Models;
using WebHackathon.Utilities;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DbHackathonContext _context;

        public BlogsController(DbHackathonContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 6;
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/blogs";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            ViewBag.page = page;

            ViewBag.page_num = (int)Math.Ceiling((decimal)_context.TbBlogs.Count() / pageSize);


            var blog = _context.TbBlogs.Include(t => t.BlogCategory).Include(t => t.User).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(blog);
        }


        public IActionResult HideShow(int id)
        {
            var blog = _context.TbBlogs.Find(id);
            if (blog != null)
            {
                blog.IsShow = !blog.IsShow;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


        

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/blogs/create";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            ViewData["BlogCategoryId"] = new SelectList(_context.TbBlogCategories, "BlogCategoryId", "Title");

            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,BlogCategoryId,UserId,Title,Description,Detail,TagBlogId,Image,Date,LikeCount,ViewCount,IsShow")] TbBlog tbBlog, IFormFile img)
        {
            tbBlog.Date = DateTime.Now;
            tbBlog.LikeCount = 0;
            tbBlog.ViewCount = 0;



            if (img != null && img.Length > 0)
            {

                var uploadPath = Path.Combine(_env.WebRootPath, "Blog");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Tạo tên file duy nhất
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var fullPath = Path.Combine(uploadPath, uniqueFileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                // Lưu đường dẫn vào database
                tbBlog.Image = "/Blog/" + uniqueFileName;

            }

            _context.TbBlogs.Add(tbBlog);
            await _context.SaveChangesAsync();
            return Redirect($"/admin/TagBlogs/{tbBlog.BlogId}");

        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = $"/admin/blogs/edit/{id}";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.TbBlogs.FindAsync(id);
            if (tbBlog == null)
            {
                return NotFound();
            }
            ViewData["BlogCategoryId"] = new SelectList(_context.TbBlogCategories, "BlogCategoryId", "BlogCategoryId", tbBlog.BlogCategoryId);
            ViewData["UserId"] = new SelectList(_context.TbUsers, "UserId", "UserId", tbBlog.UserId);
            return View(tbBlog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,BlogCategoryId,UserId,Title,Description,Detail,TagBlogId,Image,Date,LikeCount,ViewCount,IsShow")] TbBlog tbBlog, IFormFile img)
        {
            if (id != tbBlog.BlogId)
            {
                return NotFound();
            }

            if (img != null && img.Length > 0)
            {
                string imgpath = "wwwroot" + tbBlog.Image;

                if (System.IO.File.Exists(imgpath))
                {
                    System.IO.File.Delete(imgpath);
                }

                var uploadPath = Path.Combine(_env.WebRootPath, "Blog");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Tạo tên file duy nhất
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var fullPath = Path.Combine(uploadPath, uniqueFileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                // Lưu đường dẫn vào database
                tbBlog.Image = "/Blog/" + uniqueFileName;

            }

            ViewData["BlogCategoryId"] = new SelectList(_context.TbBlogCategories, "BlogCategoryId", "Title", tbBlog.BlogCategoryId);


            _context.Update(tbBlog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbBlog = await _context.TbBlogs.FindAsync(id);
            if (tbBlog != null)
            {
                _context.TbBlogs.Remove(tbBlog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbBlogExists(int id)
        {
            return _context.TbBlogs.Any(e => e.BlogId == id);
        }
    }
}
