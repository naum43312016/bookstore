using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Order
    {

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Adress { get; set; }

        public string Note { get; set; }

        [Required]
        public double Total { get; set; }

        public string Products { get; set; }

    }
}
