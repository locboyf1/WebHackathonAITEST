using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebHackathon.Models;
using WebHackathon.Utilities;
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
            if (!Function.IsLogin())
            {
                Function._message = "Please login to confirm";
                Function._returnUrl = "/admin";
                return Redirect("/login");
            }

            if (Function._userrole == 1)
            {
                Function._message = "You can't visit this site";
                return Redirect("/home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadText(string documentText)
        {
            if (string.IsNullOrWhiteSpace(documentText))
            {
                ViewBag.Message = "Please input the context.";
                return View("Index");
            }

            await _embeddingService.LoadAndSaveFromTextAsync(documentText);

            ViewBag.Message = "Succes process the content and save it!";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                ViewBag.Message = "please choose a valid pdf file .";
                return View("Index");
            }

            using var stream = pdfFile.OpenReadStream();
            using var pdf = UglyToad.PdfPig.PdfDocument.Open(stream);
            var allText = string.Join("\n", pdf.GetPages().Select(p => p.Text));

            await _embeddingService.LoadAndSaveFromTextAsync(allText);

            ViewBag.Message = "Success upload and process the pdf file.";
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
