using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebHackathon.Areas.Admin.Controllers;
using WebHackathon.Models;
using WebHackathon.Utilities;

namespace WebHackathon.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DbHackathonContext _context;
        public ProfileController(DbHackathonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bookborrow = (from b in _context.TbBorrows
                              where b.UserId == Function._userid
                              select b).ToList();
            ViewBag.BookBorrow = bookborrow;
            return View();
        }
    }
}
