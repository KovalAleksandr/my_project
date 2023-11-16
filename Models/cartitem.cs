using System.ComponentModel.DataAnnotations;
using WebApplication30.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int Quantity { get; set; }

    public Product Product { get; set; }
}
    