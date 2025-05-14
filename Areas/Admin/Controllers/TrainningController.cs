using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebHackathon.Models;
using WebHackathon.Services;

namespace WebHackathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrainningController : Controller
    {
        private readonly DbHackathonContext _context;
        private readonly ILogger<TrainningController> _logger;
        private readonly DocumentEmbeddingService _embeddingService;
        public TrainningController(ILogger<TrainningController> logger, DocumentEmbeddingService embeddingService, DbHackathonContext context)
        {
            _logger = logger;
            _embeddingService = embeddingService;
            _context = context;
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
                ViewBag.Message = "Vui lòng nhập nội dung tài liệu.";
                return View("Index");
            }

            await _embeddingService.LoadAndSaveFromTextAsync(documentText);

            ViewBag.Message = "Đã xử lý và lưu thành công embedding từ văn bản!";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                ViewBag.Message = "Vui lòng chọn tệp PDF hợp lệ.";
                return View("Index");
            }

            using var stream = pdfFile.OpenReadStream();
            using var pdf = UglyToad.PdfPig.PdfDocument.Open(stream);
            var allText = string.Join("\n", pdf.GetPages().Select(p => p.Text));

            await _embeddingService.LoadAndSaveFromTextAsync(allText);

            ViewBag.Message = "Tải lên và xử lý PDF thành công.";
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
