using WebHackathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace WebHackathon.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {

        private readonly DbHackathonContext _context;
        public HeaderViewComponent(DbHackathonContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = _context.TbMenus
                .Where(m => m.IsShow == true).OrderBy(i=>i.Position).ToList();
            return await Task.FromResult<IViewComponentResult>(View(menuItems));
        }
    }
}
