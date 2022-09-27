using BookSpot.Data;
using BookSpot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookSpot.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //pagination
            IEnumerable<Book> objBookList = _db.Books;
            var bookView = new BookView
            {
                Books = objBookList
            };
            return View(bookView);
        }


        [HttpPost]
        public async Task<IActionResult> Index(BookView obj)
        {
            IQueryable<string> genreQuery = from m in _db.Books
                                            orderby m.Genre
                                            select m.Genre;

            IEnumerable<Book> objBookList = _db.Books;


            if (!String.IsNullOrEmpty(obj.Filter.BookTitle))
            {
                objBookList = objBookList.Where(a => a.BookTitle!.Contains(obj.Filter.BookTitle));
            }

            if (!String.IsNullOrEmpty(obj.Filter.Author))
            {
                objBookList = objBookList.Where(a => a.Author!.Contains(obj.Filter.Author));
            }

            if (!String.IsNullOrEmpty(obj.Filter.Genre))
            {
                objBookList = objBookList.Where(a => a.Genre!.Contains(obj.Filter.Genre));
            }


            var bookView = new BookView
            {
                Books = objBookList,
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Filter = obj.Filter
            };

            return View(bookView);
        }

        // Add ---
        // GET
        public IActionResult Add()
        {
            return View();
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Book obj)
        { 
            if (ModelState.IsValid)
            {
                _db.Books.Add(obj);
                _db.SaveChanges();
                //TempData["success"] = "Category created sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        // Edit ---
        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var bookFormDb = _db.Books.Find(id);
            if (bookFormDb == null)
            {
                return NotFound();
            }
            return View(bookFormDb);
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order cannot be the same as the Name");
            //}
            if (ModelState.IsValid)
            {
                _db.Books.Update(obj);
                _db.SaveChanges();
                //TempData["success"] = "Category created sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        // Delete ---
        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var bookFormDb = _db.Books.Find(id);
            if (bookFormDb == null)
            {
                return NotFound();
            }
            return View(bookFormDb);
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int? id)
        {
            var obj = _db.Books.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Books.Remove(obj);
            _db.SaveChanges();
            //TempData["success"] = "Category created sucessfully";
            return RedirectToAction("Index");
        }
    }
}
