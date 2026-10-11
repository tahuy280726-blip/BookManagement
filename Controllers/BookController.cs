using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        private static readonly List<Book> Books = new()
        {
            new Book { Id = 1, Name = "Lập trình C# cơ bản", Price = 120000, Author = "Nguyễn Văn A" },
            new Book { Id = 2, Name = "ASP.NET Core MVC", Price = 180000, Author = "Trần Văn B" },
            new Book { Id = 3, Name = "Lập trình Web", Price = 150000, Author = "Lê Văn C" }
        };

        private static readonly Dictionary<string, string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg",
            [".png"] = "image/png"
        };

        public BookController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View(Books);
        }

        public IActionResult Detail(int id)
        {
            var book = Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
                return NotFound("Không tìm thấy sách.");

            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, List<IFormFile>? images)
        {
            // Kiểm tra file trước khi lưu.
            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var extension = Path.GetExtension(image.FileName);

                    if (image.Length == 0)
                    {
                        ModelState.AddModelError("", $"File {Path.GetFileName(image.FileName)} không có dữ liệu.");
                    }
                    else if (!AllowedImageTypes.TryGetValue(extension, out var contentType) ||
                             !string.Equals(image.ContentType, contentType, StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("", $"File {Path.GetFileName(image.FileName)} không hợp lệ. Chỉ được upload JPG hoặc PNG.");
                    }
                }
            }

            if (!ModelState.IsValid)
                return View(book);

            book.Id = Books.Count == 0 ? 1 : Books.Max(x => x.Id) + 1;

            // Tạo thư mục wwwroot/uploads/books nếu chưa tồn tại.
            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", "books");
            Directory.CreateDirectory(uploadFolder);

            if (images != null)
            {
                foreach (var image in images.Where(x => x.Length > 0))
                {
                    var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                    var fileName = $"{Guid.NewGuid():N}{extension}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);

                    book.ImageUrls.Add($"/uploads/books/{fileName}");
                }
            }

            Books.Add(book);
            return RedirectToAction(nameof(Index));
        }
    }
}
