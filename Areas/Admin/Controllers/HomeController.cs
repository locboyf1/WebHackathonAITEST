using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebHackathon.Models;
using WebHackathon.Services;
using WebHackathon.Utilities;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DbHackathonContext _context;

        public HomeController(DbHackathonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            return Redirect(Function.CheckAdmin(Url.Action("Index")));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
