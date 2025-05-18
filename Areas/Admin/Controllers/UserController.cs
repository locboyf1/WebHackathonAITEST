using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Models;
using WebHackathon.Utilities;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DbHackathonContext _context;

        public UserController(DbHackathonContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IActionResult> ChangeRole(int id, int role)
        {
            var user = await _context.TbUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (Function._userrole != 2)
            {
                Function._message = "You do not have permission";
            }
            else
            {
                user.RoleId = role;
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> LockUnlock(int id)
        {

            var user = await _context.TbUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (Function._userid == 3 && user.RoleId == 2)
            {
                Function._message = "You do not have permission";
            }
            user.IsLock = !user.IsLock;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/User
        public async Task<IActionResult> Index(int page = 1)
        {

            const int pageSize = 10;

            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/User";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            var users = await _context.TbUsers
                .Include(u => u.Role)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).Where(i => i.UserId != Function._userid)
                .ToListAsync();

            ViewBag.page = page;
            ViewBag.page_num = (int)Math.Ceiling((double)_context.TbUsers.Count() / pageSize);

            return View(users);
        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = $"/admin/User/edit/{id}";
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

            var tbUser = await _context.TbUsers.FindAsync(id);
            if (tbUser == null)
            {
                return NotFound();
            }
            return View(tbUser);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password,RoleId,Name,Balance,Avatar,IsLock")] TbUser tbUser, IFormFile img)
        {
            if (id != tbUser.UserId)
            {
                return NotFound();
            }


            if (img != null && img.Length > 0)
            {
                string imgpath = "wwwroot" + tbUser.Avatar;

                if (System.IO.File.Exists(imgpath) && tbUser.Avatar != "/assets/images/DefaultAvatar.jpg")
                {
                    System.IO.File.Delete(imgpath);
                }

                var uploadPath = Path.Combine(_env.WebRootPath, "Avatar");
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
                tbUser.Avatar = "/Avatar/" + uniqueFileName;

            }

            _context.Update(tbUser);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        private bool TbUserExists(int id)
        {
            return _context.TbUsers.Any(e => e.UserId == id);
        }
    }
}