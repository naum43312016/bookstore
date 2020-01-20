using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModel;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminOrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Order> orders = await _db.Order.ToListAsync();
            List<AdminOrderViewModel> orderVm = new List<AdminOrderViewModel>();
            if(orders==null || orders.Count() < 1)
            {
                return View(null);
            }
            for(int i = 0; i < orders.Count(); i++)
            {
                AdminOrderViewModel orVm = new AdminOrderViewModel();
                orVm.order = orders[i];
                List<CartModel> list = new List<CartModel>();
                list = SerializeObject.DeSerializeObjectFromString<List<CartModel>>(orders[i].Products,list);
                List<Book> books = new List<Book>();
                List<string> count = new List<string>();
                for(int j = 0; j < list.Count(); j++)
                {
                    books.Add(await _db.Book.FindAsync(list[j].productId));
                    count.Add(list[j].count);
                }
                orVm.books = books;
                orVm.count = count;
                orderVm.Add(orVm);

            }
            return View(orderVm);
        }
    }
}