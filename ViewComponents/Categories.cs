using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHackathon.Models;

namespace WebHackathon.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly DbHackathonContext _context;

        public Categories(DbHackathonContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.TbCategories
                .OrderBy(c => c.CategoryId)
                .ToListAsync();

            return View(categories);
        }
    }
}