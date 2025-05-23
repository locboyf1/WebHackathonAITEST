﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UglyToad.PdfPig.Content;
using WebHackathon.Models;
using WebHackathon.Utilities;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogCategoriesController : Controller
    {
        private readonly DbHackathonContext _context;

        public BlogCategoriesController(DbHackathonContext context)
        {
            _context = context;
        }

        // GET: Admin/BlogCategories
        public async Task<IActionResult> Index(int page = 1)
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/BLogcategories";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            const int pageSize = 10;

            ViewBag.page = page;

            ViewBag.page_num = (int)Math.Ceiling((double)_context.TbAuthors.Count() / pageSize);

            var category = _context.TbBlogCategories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(category);
        }


        // GET: Admin/BlogCategories/Create
        public IActionResult Create()
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/Blogcategories/create";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }
            return View();
        }

        // POST: Admin/BlogCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogCategoryId,Title,Description")] TbBlogCategory tbBlogCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbBlogCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbBlogCategory);
        }

        // GET: Admin/BlogCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = $"/admin/BlogCategories/{id}";
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

            var tbBlogCategory = await _context.TbBlogCategories.FindAsync(id);
            if (tbBlogCategory == null)
            {
                return NotFound();
            }
            return View(tbBlogCategory);
        }

        // POST: Admin/BlogCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogCategoryId,Title,Description")] TbBlogCategory tbBlogCategory)
        {
            if (id != tbBlogCategory.BlogCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbBlogCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBlogCategoryExists(tbBlogCategory.BlogCategoryId))
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
            return View(tbBlogCategory);
        }

    

        // POST: Admin/BlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin/BLogcategories";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }

            var tbBlogCategory = await _context.TbBlogCategories.FindAsync(id);
            if (tbBlogCategory != null)
            {
                _context.TbBlogCategories.Remove(tbBlogCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbBlogCategoryExists(int id)
        {
            return _context.TbBlogCategories.Any(e => e.BlogCategoryId == id);
        }
    }
}
