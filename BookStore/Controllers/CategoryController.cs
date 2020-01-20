using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _db.Category.ToListAsync();
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            Category category = new Category()
            {
                Name = name
            };
            await _db.Category.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }
        public async Task<IActionResult> Remove(int id)
        {

            Category cat = await _db.Category.FindAsync(id);
            if (cat != null)
            {
                _db.Category.Remove(cat);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string categoryId, string name)
        {
            if (categoryId != null)
            {
                var Id = Int32.Parse(categoryId);
                Category cat = await _db.Category.FindAsync(Id);
                if (cat != null)
                {
                    cat.Name = name;
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Category");
        }
    }
}