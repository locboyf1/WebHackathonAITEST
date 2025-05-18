using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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
        public IActionResult Details(int id) {

            var user = _context.TbUsers.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy
            }

            return View(user); // Truyền user vào View
        }

        [HttpPost]
        public IActionResult UpdateProfile(TbUser model) {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Thông tin nhập không hợp lệ.";
                return View(model);
            }

            try
            {

                // Tìm user trong DB
                var user = _context.TbUsers.FirstOrDefault(u => u.UserId == Function._userid);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy người dùng.";
                    return View(model);
                }
                  user.Name = model.Name;
                  user.Email = model.Email;
                _context.SaveChanges();
                TempData["Success"] = "Cập nhật thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex) {

                TempData["Error"] = "Có lỗi xảy ra khi cập nhật: " + ex.Message;
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePass(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập đầy đủ thông tin.");
                return RedirectToAction("Details", new { id = Function._userid }); // Giữ lại view và hiển thị lỗi
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không khớp.");
                return RedirectToAction("Details", new { id = Function._userid });
            }

            
            var user = _context.TbUsers.FirstOrDefault(u => u.UserId == Function._userid);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy người dùng.");
                return RedirectToAction("Details", new { id = Function._userid });
            }

            var hashedCurrent = Function.HashPassword(currentPassword);
            if (user.Password != hashedCurrent)
            {
                ModelState.AddModelError("currentPassword", "Mật khẩu hiện tại không đúng.");
                return RedirectToAction("Details", new { id = Function._userid });
            }

            user.Password = Function.HashPassword(newPassword);
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật mật khẩu thành công.";
            return RedirectToAction("Index");
        }



    }
}
