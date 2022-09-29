using BookSpot.Data;
using BookSpot.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookSpot.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<AppUser> _userManager { get; }
        public bool IsUserSignedIn() => (User.Identity != null) && (User.Identity.IsAuthenticated);


        public BookController(ApplicationDbContext db, UserManager<AppUser> userMan)
        {
            _db = db;
            _userManager = userMan;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> objBookList;
            if (IsUserSignedIn()) { 
                var user = _userManager.GetUserAsync(User).Result;
                ViewBag.role = user?.AppUserRole;

                if (_userManager.GetUserAsync(User).Result.AppUserRole == RoleModel.Admin)
                {
                    objBookList = _db.Books;
                }
                else
                {
                    var foreignReservedBooks = _db.Reservations.Where(x => x.AppUserId != user.Id).Select(x => x.BookId).ToList();
                    var books = _db.Books.Where(x => foreignReservedBooks.Contains(x.Id) == false);

                    objBookList = books;
                }
            }
            else 
            {
                objBookList = _db.Books;
            }
           
            var bookView = new BookView
            {
                Books = objBookList,
                Filter = null
            };
            return View(bookView);
        }

        // Filter
        // POST
        [HttpPost]
        public async Task<IActionResult> FilterBooks(BookView bookFilter)
        {
            IEnumerable<Book> objBookList;
            if (IsUserSignedIn())
            {
                var user = _userManager.GetUserAsync(User).Result;
                ViewBag.role = user?.AppUserRole;

                if (_userManager.GetUserAsync(User).Result.AppUserRole == RoleModel.Admin)
                {
                    objBookList = _db.Books;
                }
                else
                {
                    var foreignReservedBooks = _db.Reservations.Where(x => x.AppUserId != user.Id).Select(x => x.BookId).ToList();
                    var books = _db.Books.Where(x => foreignReservedBooks.Contains(x.Id) == false);

                    objBookList = books;
                }
            }
            else
            {
                objBookList = _db.Books;
            }

            if (bookFilter.Filter?.BookTitle != null)
            {
                objBookList = objBookList.Where(x => x.BookTitle.ToLower().Contains(bookFilter.Filter.BookTitle.ToLower()));
            }

            if(bookFilter.Filter?.Genre != null)
            {
                objBookList = objBookList.Where(x => x.Genre.ToLower().Contains(bookFilter.Filter.Genre.ToLower()));
            }

            if (bookFilter.Filter?.Publisher != null)
            {
                objBookList = objBookList.Where(x => x.Publisher.ToLower().Contains(bookFilter.Filter.Publisher.ToLower()));
            }

            if (bookFilter.Filter?.Author != null)
            {
                objBookList = objBookList.Where(x => x.Author.ToLower().Contains(bookFilter.Filter.Author.ToLower()));
            }

            if (bookFilter.Filter?.ISBNcode != null)
            {
                objBookList = objBookList.Where(x => x.ISBNcode.Equals(bookFilter.Filter.ISBNcode));
            }

            if (bookFilter.Filter?.Status != null)
            {
                objBookList = objBookList.Where(x => x.Status.Equals(bookFilter.Filter.Status));
            }

            if (bookFilter.Filter?.PublishingDate != DateTime.MinValue)
            {
                objBookList = objBookList.Where(x => x.PublishingDate.Date.Equals(bookFilter.Filter.PublishingDate.Date));
            }

            var bookView = new BookView
            {
                Books = objBookList,
                Filter = bookFilter.Filter
            };

            return View("Index", bookView);
        }
        // Reserve
        // GET
        public IActionResult ReserveBook (int id)
        {
            if (IsUserSignedIn())
            {
                var updateBook = new Reservation()
                {
                    BookId = id,
                    AppUserId = _userManager.GetUserId(User)
                };
                var res = _db.Reservations.Where(x => x.BookId == id && x.AppUserId == updateBook.AppUserId).FirstOrDefault();
                if(res != null)
                {
                    TempData["error"] = "Book not reserved";
                    return RedirectToAction("Index");
                }
                _db.Reservations.Add(updateBook);
                var book = _db.Books.Where(x => x.Id.Equals(id) && x.Status == BookStatus.Available).FirstOrDefault();
                if (book != null)
                {
                    book.Status = BookStatus.Reserved;
                    _db.Books.Update(book);
                    _db.SaveChanges();
                    TempData["success"] = "Book reserved sucessfully";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "Book not reserved";

            }
            return RedirectToAction("Index");
        }
        // Borrow
        // GET
        public IActionResult BorrowBook(int id)
        {
            if (IsUserSignedIn())
            {
                var updateBook = new Reservation()
                {
                    BookId = id,
                    AppUserId = _userManager.GetUserId(User)
                };
                var res = _db.Reservations.Where(x => x.BookId == id && x.AppUserId == updateBook.AppUserId ).FirstOrDefault();
                if (res == null)
                {
                    TempData["error"] = "Book cannot be borrowed";
                    return RedirectToAction("Index");
                }
                var book = _db.Books.Where(x => x.Id.Equals(res.BookId) && x.Status == BookStatus.Reserved).FirstOrDefault();
                if (book != null)
                {
                    book.Status = BookStatus.Borrowed;
                    _db.Books.Update(book);
                    _db.SaveChanges();
                    TempData["success"] = "Book borrowed sucessfully";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "Book cannot be borrowed";

            }
            return RedirectToAction("Index");
        }
        // Return
        // GET
        public IActionResult ReturnBook(int id)
        {
            if (IsUserSignedIn())
            {
                var res = _db.Reservations.Where(x => x.BookId == id).FirstOrDefault();

                if (res == null)
                {
                    TempData["error"] = "Book cannot be returned";
                    return RedirectToAction("Index");
                }
                _db.Reservations.Remove(res);
                var book = _db.Books.Where(x => x.Id.Equals(res.BookId)).FirstOrDefault();
                if (book != null)
                {
                    book.Status = BookStatus.Available;
                    _db.Books.Update(book);
                    _db.SaveChanges();
                    TempData["success"] = "Book returned sucessfully";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "Book cannot be returned";

            }
            return RedirectToAction("Index");
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
                TempData["success"] = "Book created sucessfully";
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
            if (ModelState.IsValid)
            {
                _db.Books.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Book edited sucessfully";
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

            var res = _db.Reservations.Where(x => x.BookId == id).FirstOrDefault();
            if (res != null)
            {
                _db.Reservations.Remove(res);
            }

            _db.Books.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Book deleted sucessfully";
            return RedirectToAction("Index");
        }
    }
}