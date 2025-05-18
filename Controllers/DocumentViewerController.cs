using GroupDocs.Viewer.Options;
using GroupDocs.Viewer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using WebHackathon.Models;
using WebHackathon.Utilities;

namespace WebHackathon.Controllers
{
    public class DocumentViewerController : Controller
    {
        private readonly DbHackathonContext _db;
       
        private readonly IWebHostEnvironment _hostingEnvironment;

        //public DocumentViewerController(IWebHostEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        public IActionResult Index()
        {
            string documentsPath = Path.Combine(_env.WebRootPath, "Documents");

            if (!Directory.Exists(documentsPath))
            {
                return View("Index", new List<string>());
            }

            var pdfFiles = Directory.GetFiles(documentsPath, "*.pdf")
                .Select(Path.GetFileName)
                .ToList();

            return View("Index", pdfFiles);
        }
        /*  public IActionResult ViewDocument(string fileName)
          {
              try
              {
                  if (string.IsNullOrEmpty(fileName))
                  {
                      return BadRequest("File name is required");
                  }

                  string documentsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                  string filePath = Path.Combine(documentsDirectory, fileName);

                  if (!System.IO.File.Exists(filePath))
                  {
                      return NotFound($"File {fileName} not found");
                  }

                  // Create output directory for the converted HTML pages
                  string outputDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "DocumentViewer", Path.GetFileNameWithoutExtension(fileName));

                  // Delete the directory if it exists to avoid any conflicts with previous conversions
                  if (Directory.Exists(outputDirectory))
                  {
                      Directory.Delete(outputDirectory, true);
                  }

                  Directory.CreateDirectory(outputDirectory);

                  // Convert the document to HTML
                  using (Viewer viewer = new Viewer(filePath))
                  {
                      HtmlViewOptions options = HtmlViewOptions.ForEmbeddedResources(
                          Path.Combine(outputDirectory, "page_{0}.html"));

                      // Add additional options that might improve rendering
                      options.RenderResponsive = true;
                      //options.IgnoreResourcePrefixForCss = true; // Helps with CSS loading
                      options.RenderResponsive = false;
                      // Perform the conversion
                      viewer.View(options);
                  }

                  // Verify pages were generated
                  string[] generatedPages = Directory.GetFiles(outputDirectory, "page_*.html");
                  if (generatedPages.Length == 0)
                  {
                      return StatusCode(500, "Không thể chuyển đổi tài liệu - không có trang nào được tạo ra");
                  }

                  // Pass necessary information to the view
                  ViewBag.FileName = fileName;
                  ViewBag.PageCount = generatedPages.Length;
                  ViewBag.OutputDir = Path.GetFileNameWithoutExtension(fileName);

                  // Return the Viewer view
                  return View("Viewer");
              }
              catch (Exception ex)
              {
                  // Log the error and return an error view
                  return View("Error", new ErrorViewModel
                  {
                      RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                      //ErrorMessage = $"Lỗi khi xử lý tài liệu: {ex.Message}"
                  });
              }
          }*/
        private readonly IWebHostEnvironment _env;

        public DocumentViewerController(IWebHostEnvironment env, DbHackathonContext db)
        {
            _env = env;
            _db = db;
        }

        public IActionResult ViewPdf(int? id, string returnUrl)
        {
            var fileName = (from b in _db.TbBooks
                            where b.BookId == id
                            select b).FirstOrDefault();
            if (string.IsNullOrEmpty(fileName.BookPdf))
                return BadRequest("Tên file không được rỗng.");

            string filePath = Path.Combine(_env.WebRootPath, "Documents", fileName.BookPdf);
            if (!System.IO.File.Exists(filePath))
                return NotFound($"Không tìm thấy file: {fileName.BookPdf}");

            

            bool hasBorrowed = _db.TbBorrows.Any(b => b.BookId == id && b.UserId == Function._userid);
            ViewBag.FileName = fileName.BookPdf;
            ViewBag.PreviewOnly = !hasBorrowed;
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            return View("Viewer");
        }

        public IActionResult Viewer()
        {
            return View();
        }
        // Nếu bạn muốn thêm endpoint để lấy một trang cụ thể
        public IActionResult GetPage(string fileName, int pageNumber)
        {
            string outputDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "DocumentViewer",
                Path.GetFileNameWithoutExtension(fileName));
            string pagePath = Path.Combine(outputDirectory, $"page_{pageNumber}.html");

            if (!System.IO.File.Exists(pagePath))
            {
                return NotFound($"Trang {pageNumber} không tồn tại");
            }

            return File(System.IO.File.ReadAllBytes(pagePath), "text/html");
        }
    }
}