using Microsoft.AspNetCore.Mvc;
using WebHackathon.Utilities;
using WebHackathon.Models;

namespace WebHackathon.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DbHackathonContext _context;

        public RegisterController(DbHackathonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(TbUser user)
        {
            var hashedPassword = Function.HashPassword(user.Password);
            var check = _context.TbUsers.FirstOrDefault(u => u.Email == user.Email);
            if (check != null)
            {
                Function._message = "Email already exists";
                return View("Index");
            }

            user.Balance = 0;
            user.IsLock = false;
            user.RoleId = 1;
            user.Password = hashedPassword; 
            user.Avatar = "/assets/images/DefaultAvatar.jpg";
            Function._message = "Register successfully";

            _context.TbUsers.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Login");
        }
    }
}
