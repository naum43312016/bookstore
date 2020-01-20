using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModel
{
    public class AdminOrderViewModel
    {
        public Order order { get; set; }
        public List<Book> books { get; set; }
        public List<string> count { get; set; }
    }
}
