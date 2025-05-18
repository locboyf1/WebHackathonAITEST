using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            int count;
            if (Request.Cookies.TryGetValue("wrong", out string wrong))
            {
                count = int.Parse(wrong);
            }
            else
            {
                count = 0;

            }

            if (check == null)
            {

                count++;
                Response.Cookies.Append("wrong", count.ToString(), new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true

                });
                Function._message = "Email or Password is incorrect";
                return View("Index");
            }
            else
            {
                Response.Cookies.Append("wrong", "1", new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true

                });

                if (check.IsLock == true)
                {
                    Function._message = "Your account is locked";
                    return View("Index");
                }
                if (count >= 2)
                {
                    Function._message = "You have logged in incorrectly too many times.";
                    return View("Index");
                }

                Function._useremail = check.Email;
                Function._username = check.Name;
                Function._userid = check.UserId;
                Function._useravatar = check.Avatar;
                Function._userrole = check.RoleId;

                if (string.IsNullOrEmpty(Function._returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }

                string link = Function._returnUrl;
                Function._returnUrl = string.Empty;

                return Redirect(link);

            }

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
