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
        private readonly DbHackathonContext _context;

        public UserController(DbHackathonContext context)
        {
            _context = context;
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
                .Take(pageSize)
                .ToListAsync();

            ViewBag.page = page;
            ViewBag.page_num = (int)Math.Ceiling((double)_context.TbUsers.Count() / pageSize);

            return View(users);
        }

        // GET: Admin/User/Create
        public IActionResult Create()
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/User/create";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,Password,RoleId,Name,Balance,Avatar,IsLock")] TbUser tbUser)
        {
            if (ModelState.IsValid)
            {
                Function._message = "Add user successfully";
                _context.Add(tbUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbUser);
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
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password,RoleId,Name,Balance,Avatar,IsLock")] TbUser tbUser)
        {
            if (id != tbUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbUserExists(tbUser.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbUser);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbUser = await _context.TbUsers
                .Include(u => u.TbBlogs)
                .Include(u => u.TbBorrows)
                .Include(u => u.TbCarts)
                .Include(u => u.TbDownloadeds)
                .Include(u => u.TbUserFiles)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (tbUser != null)
            {
                _context.TbUsers.Remove(tbUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbUserExists(int id)
        {
            return _context.TbUsers.Any(e => e.UserId == id);
        }
    }
}