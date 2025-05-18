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
    public class PublishersController : Controller
    {
        private readonly DbHackathonContext _context;

        public PublishersController(DbHackathonContext context)
        {
            _context = context;
        }

        // GET: Admin/Publishers
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;

            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/Publishers";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            var publisher = await _context.TbPublishers.Skip((1 - 1) * pageSize).ToListAsync();

            ViewBag.page = page;

            ViewBag.page_num = (int)Math.Ceiling((double)_context.TbAuthors.Count() / pageSize);
            return View(publisher);
        }


        // GET: Admin/Publishers/Create
        public IActionResult Create()
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/Publishers/create";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }
            return View();
        }

        // POST: Admin/Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublisherId,Name,Description")] TbPublisher tbPublisher)
        {
            if (ModelState.IsValid)
            {
                Function._message = "Add publisher successfully";
                _context.Add(tbPublisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbPublisher);
        }

        // GET: Admin/Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = $"/admin/Publishers/edit/{id}";
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

            var tbPublisher = await _context.TbPublishers.FindAsync(id);
            if (tbPublisher == null)
            {
                return NotFound();
            }
            return View(tbPublisher);
        }

        // POST: Admin/Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublisherId,Name,Description")] TbPublisher tbPublisher)
        {
            if (id != tbPublisher.PublisherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbPublisher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbPublisherExists(tbPublisher.PublisherId))
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
            return View(tbPublisher);
        }

        // POST: Admin/Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbPublisher = await _context.TbPublishers.Include(i => i.TbBooks).ThenInclude(i => i.TbDownloadeds).FirstOrDefaultAsync(i => i.PublisherId == id);
            if (tbPublisher != null)
            {
                _context.TbPublishers.Remove(tbPublisher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbPublisherExists(int id)
        {
            return _context.TbPublishers.Any(e => e.PublisherId == id);
        }
    }
}
