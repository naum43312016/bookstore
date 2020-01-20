using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModel
{
    public class CartViewModel
    {
        public Book Book { get; set; }
        public int Count { get; set; }
    }
}
