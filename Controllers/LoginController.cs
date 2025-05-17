using Microsoft.AspNetCore.Mvc;
using WebHackathon.Models;
using WebHackathon.Utilities;


namespace WebHackathon.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbHackathonContext _context;

        public LoginController(DbHackathonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var hashedPassword = Function.HashPassword(password);
            var check = _context.TbUsers.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);
            if (check == null)
            {
                Function._message = "Email or Password is incorrect";
                return View("Index");
            }

            if (check.IsLock == true)
            {
                Function._message = "Your account is locked";
                return View("Index");
            }

            Function._useremail = check.Email;
            Function._username = check.Name;
            Function._userid = check.UserId;
            Function._useravatar = check.Avatar;
            Function._userrole = check.RoleId;

            if(string.IsNullOrEmpty(Function._returnUrl)){
                return RedirectToAction("Index", "Home");
            }

            string link = Function._returnUrl;
            Function._returnUrl = string.Empty;

            return Redirect(link);
        }

        public IActionResult Logout()
        {
            Function._useremail = string.Empty;
            Function._username = string.Empty;
            Function._userid = 0;
            Function._userrole = 0;
            Function._useravatar = string.Empty;
            Function._message = "Logout success!";
            return RedirectToAction("Index", "Home");
        }
    }
}
