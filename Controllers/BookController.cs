using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        private static readonly List<Book> Books = new()
        {
            new Book { Id = 1, Name = "Lập trình C# cơ bản", Price = 120000, Author = "Nguyễn Văn A" },
            new Book { Id = 2, Name = "ASP.NET Core MVC", Price = 180000, Author = "Trần Văn B" },
            new Book { Id = 3, Name = "Lập trình Web", Price = 150000, Author = "Lê Văn C" }
        };

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
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            book.Id = Books.Count == 0 ? 1 : Books.Max(x => x.Id) + 1;
            Books.Add(book);
            return RedirectToAction(nameof(Index));
        }
    }
}
