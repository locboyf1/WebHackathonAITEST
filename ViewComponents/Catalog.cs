using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Models;

namespace WebHackathon.ViewComponents
{
    public class Catalog : ViewComponent
    {
        private readonly DbHackathonContext _context;

        public Catalog(DbHackathonContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var catalog = await _context.TbBlogCategories
                .OrderBy(c => c.BlogCategoryId)
                .ToListAsync();

            return View("Default", catalog);
        }
    }
}