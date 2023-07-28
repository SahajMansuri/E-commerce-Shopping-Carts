using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Card.Models
{

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [RegularExpression(@"^\d{1,5}$", ErrorMessage = "Price must be a number with a maximum length of 5 digits.")]
        public int ProductPrice { get; set; }

        public byte[] ImageData { get; set; }


        public string ProductImage { get; set; }
    }
}