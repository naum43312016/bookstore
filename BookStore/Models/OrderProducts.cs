using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class OrderProducts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        [Required]
        public string Count { get; set; }
    }
}
