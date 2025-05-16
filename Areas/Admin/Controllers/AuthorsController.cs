using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Utilities;
using System.IO;
using WebHackathon.Models;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DbHackathonContext _context;

        public AuthorsController(DbHackathonContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Authors
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 6;
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/authors";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            ViewBag.page = page;

            ViewBag.page_num = (int)Math.Ceiling((double)_context.TbAuthors.Count() / pageSize);

            var authorlist = await _context.TbAuthors.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return View(authorlist);
        }

        // GET: Admin/Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = $"/admin/authors/details/{id}";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            var tbAuthor = await _context.TbAuthors
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (tbAuthor == null)
            {
                return NotFound();
            }

            return View(tbAuthor);
        }

        // GET: Admin/Authors/Create
        public IActionResult Create()
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/authors/create";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,Name,Description,Image")] TbAuthor tbAuthor, IFormFile img)
        {
            if (img != null && img.Length > 0)
            {

                var uploadPath = Path.Combine(_env.WebRootPath, "Author");
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
                tbAuthor.Image = "/Author/" + uniqueFileName;

            }

            Function._message = "Create author successfully";
            _context.TbAuthors.Add(tbAuthor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = $"/admin/authors/edit/{id}";
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

            var tbAuthor = await _context.TbAuthors.FindAsync(id);
            if (tbAuthor == null)
            {
                return NotFound();
            }
            return View(tbAuthor);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Name,Description,Image")] TbAuthor tbAuthor, IFormFile img)
        {
            if (id != tbAuthor.AuthorId)
            {
                return NotFound();
            }

            if (img != null && img.Length > 0)
            {
                string imgpath = "wwwroot" + tbAuthor.Image;

                if (System.IO.File.Exists(imgpath))
                {
                    System.IO.File.Delete(imgpath);
                }

                var uploadPath = Path.Combine(_env.WebRootPath, "Author");
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
                tbAuthor.Image = "/Author/" + uniqueFileName;

            }
            _context.TbAuthors.Update(tbAuthor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // POST: Admin/Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbAuthor = await _context.TbAuthors.Include(i=>i.TbBooks).ThenInclude(i=>i.TbCarts).FirstOrDefaultAsync(i=>i.AuthorId == id);

            if (tbAuthor != null)
            {
                string img = "wwwroot" + tbAuthor.Image;
                if (System.IO.File.Exists(img))
                {
                    System.IO.File.Delete(img);
                }
                _context.TbAuthors.Remove(tbAuthor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAuthorExists(int id)
        {
            return _context.TbAuthors.Any(e => e.AuthorId == id);
        }
    }
}
