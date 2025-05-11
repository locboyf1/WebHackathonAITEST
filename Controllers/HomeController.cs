using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebHackathon.Models;

namespace WebHackathon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DocumentEmbeddingService _embedding;
        public HomeController(ILogger<HomeController> logger, DocumentEmbeddingService embedding)
        {
            _logger = logger;
            _embedding = embedding;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadText(string documentText)
        {
            if (string.IsNullOrWhiteSpace(documentText))
            {
                ModelState.AddModelError("", "Vui lòng nhập nội dung tài liệu.");
                return View("Index");
            }

            await _embedding.LoadAndSaveAsync(documentText);

            ViewBag.Message = "Đã xử lý và lưu thành công embedding!";
            return View("Index");
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
