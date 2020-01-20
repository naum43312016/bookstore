using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("GetAllCategories")]
        [HttpGet]
        public ActionResult Get()
        {
            IEnumerable<Category> categories = _db.Category.ToList();
            //var data = JsonConvert.SerializeObject(new { data = categories });
            return Ok(categories);
            //return Content(JsonConvert.SerializeObject(new { data = categories }), "text/javascript");
        }


        [Route("addToCart")]
        [HttpPost]
        public bool Add([FromBody]CartModel obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") == null)
                {
                    List<CartModel> list = new List<CartModel>();
                    CartModel c1 = new CartModel()
                    {
                        productId = obj.productId,
                        count = obj.count
                    };
                    list.Add(c1);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
                }
                else
                {
                    List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].productId == obj.productId)
                        {
                            list[i].count += obj.count;
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
                            return true;
                        }
                    }
                    CartModel c1 = new CartModel()
                    {
                        productId = obj.productId,
                        count = obj.count
                    };
                    list.Add(c1);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
                }
                return true;
            }
        }

    }
}
