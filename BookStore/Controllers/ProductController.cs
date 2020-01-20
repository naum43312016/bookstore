using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ProductController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
    }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> products = await _db.Book.Include(c=>c.Category).ToListAsync();
            return View(products);
        }

        
        public IActionResult Create()
        {
            IEnumerable<Category> categories = _db.Category.ToList();
            ViewData["categories"] = categories;
            return View(new Book());
        }


        public IActionResult Remove(int id)
        {
            Book book = _db.Book.Find(id);
            _db.Remove(book);
            _db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Edit(int id)
        {
            Book book = _db.Book.Find(id);
            IEnumerable<Category> categories = _db.Category.ToList();
            ViewData["categories"] = categories;
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book model, string price, IFormFile photos)
        {
            model.Price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture);
            model.Category = await _db.Category.FindAsync(model.CategoryId);
            if (photos != null)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                //Delete cuurent product image
                string _imageToDelete = Path.Combine(webRootPath, model.Image);
                if ((System.IO.File.Exists(_imageToDelete)))
                {
                    System.IO.File.Delete(_imageToDelete);
                    
                }


                string imgName = model.Id.ToString() + photos.FileName;
                var path = Path.Combine(webRootPath, "img", imgName);
                var stream = new FileStream(path, FileMode.Create);
                await photos.CopyToAsync(stream);
                model.Image = "img/" + imgName;
                stream.Close();
            }
            _db.Book.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model, string price,IFormFile photos)
        {
            model.Price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture);
            model.Category = await _db.Category.FindAsync(model.CategoryId);
            model.Rating = 0.0;
            model.ReviewsCount = 0;
            model.Image = "";
            await _db.Book.AddAsync(model);
            await _db.SaveChangesAsync();
            if (photos != null)
            {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string imgName = model.Id.ToString() + photos.FileName;
                    var path = Path.Combine(webRootPath, "img", imgName);
                    var stream = new FileStream(path, FileMode.Create);
                    await photos.CopyToAsync(stream);
                    model.Image = "img/" + imgName;
                    stream.Close();
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }
    }
}