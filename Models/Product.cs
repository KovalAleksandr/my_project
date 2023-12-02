using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication30.Models
{
    public class Product
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название продукта")]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImgUrl { get; set; }

        public int Discount { get; set; }

        public string Description { get; set; }

        public string CardText { get; set; }

        public string CardBackground { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        public List<CartItem> CartItems { get; set; }
    }
}

 
