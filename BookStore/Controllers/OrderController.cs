using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {


        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAsync(Order order)
        {
            List<CartModel> list = null;
            //Get products from cart
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
               list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
            }
            if(list==null || list.Count() < 1)
            {
                return View(0);
            }
            string serProducts = SerializeObject.SerializeObjectToString<List<CartModel>>(list);
            if(serProducts==null || serProducts.Length < 1)
            {
                return View(0);
            }
            order.Products = serProducts;
            await _db.Order.AddAsync(order);
            await _db.SaveChangesAsync();
            if (order.Id == 0)
            {
                return View(0);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", null);
            return View(order.Id);
        }
    }
}