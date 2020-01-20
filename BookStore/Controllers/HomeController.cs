using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookStore.Models;
using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using BookStore.Utility;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db,ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page)
        {
            int perPage = 6;
            if (page == 0) page = 1;
            int count = _db.Book.Count();
            int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
            if (page > pagesCount) page = 1;
            ViewData["currentPage"] = page;
            ViewData["pagesCount"] = pagesCount;
            IEnumerable<Book> books = await _db.Book.Include(c => c.Category).OrderByDescending(i => i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return View(books);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> ProductDetails(int id)
        {
            Book book = await _db.Book.FindAsync(id);   
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [HttpGet]
        public async Task<IActionResult> Search(string searchInput, int page)
        {
            int perPage = 6;
            if (page == 0) page = 1;


            ViewData["currentPage"] = page;

            IEnumerable<Book> books = await _db.Book.Where(c => c.Name.Contains(searchInput)).OrderByDescending(i => i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToListAsync();

            int count = _db.Book.Where(c => c.Name.Contains(searchInput)).Count();

            int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
            ViewData["pagesCount"] = pagesCount;
            return View(books);
        }


        public async Task<IActionResult> Category(int id,int page)
        {
            int perPage = 6;
            if (page == 0) page = 1;
            
            
            ViewData["currentPage"] = page;
            
            IEnumerable<Book> books = await _db.Book.Where(c=>c.CategoryId==id).OrderByDescending(i => i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToListAsync();

            int count = _db.Book.Where(c => c.CategoryId == id).Count();

            int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
            ViewData["pagesCount"] = pagesCount;
            Category cat = await _db.Category.FindAsync(id);
            ViewData["category"] = cat.Name;
            return View(books);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
