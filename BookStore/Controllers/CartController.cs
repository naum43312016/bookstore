using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModel;
using BookStore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {


        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<CartViewModel> cartVM = new List<CartViewModel>();
            double total = 0;
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                for (int i = 0; i < list.Count; i++)
                {
                    Book pr = await _db.Book.Include(c=>c.Category).Where(c=>c.Id==list[i].productId).FirstOrDefaultAsync();
                    total += pr.Price * Int32.Parse(list[i].count);
                    CartViewModel c = new CartViewModel()
                    {
                        Book = pr,
                        Count = Int32.Parse(list[i].count)
                    };
                    cartVM.Add(c);
                }
            }
            ViewData["total"] = Math.Round(total, 2);
            return View(cartVM);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string note)
        {
            
            List<CartViewModel> cartVM = new List<CartViewModel>();
            double total = 0;
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                for (int i = 0; i < list.Count; i++)
                {
                    Book pr = await _db.Book.Include(c => c.Category).Where(c => c.Id == list[i].productId).FirstOrDefaultAsync();
                    total += pr.Price * Int32.Parse(list[i].count);
                    CartViewModel c = new CartViewModel()
                    {
                        Book = pr,
                        Count = Int32.Parse(list[i].count)
                    };
                    cartVM.Add(c);
                }
            }
            ViewData["total"] = Math.Round(total, 2);
            ViewData["note"] = note;
            ViewData["cartVM"] = cartVM;

            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.UserName; // will give the user's Email
            string id = applicationUser?.Id;
            Order order = new Order();
            if (userEmail != null)
            {
                order.Email = userEmail;
                order.UserId = id;
            }
            

            return View(order);
        }
        
    }
}