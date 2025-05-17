using Microsoft.AspNetCore.Mvc;
using WebHackathon.Models;
using WebHackathon.Utilities;

namespace WebHackathon.Controllers
{
    public class ContactController : Controller
    {
        private readonly DbHackathonContext _context;
        public ContactController(DbHackathonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(TbContact contact)
        {
            contact.IsResponded = false;
            if (ModelState.IsValid)
            {
                _context.TbContacts.Add(contact);
                _context.SaveChanges();
                Function._message = "Your message has been sent successfully.";

                return Redirect("/home");
            }
            return View();
        }

    }
}
