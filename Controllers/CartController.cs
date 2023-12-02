using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using WebApplication30.Data;
    using WebApplication30.Models;

    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();
            var products = _context.Products.ToList();

            decimal totalPrice = 0;
            foreach (var cartItem in cartItems)
            {
                var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);
                if (product != null)
                {
                    decimal discountedPrice = product.Price * (100 - product.Discount) / 100;
                    totalPrice += discountedPrice * cartItem.Quantity;
                    cartItem.Product = product; 
                    cartItem.Product.DiscountedPrice = discountedPrice; 
                    cartItem.Product.TotalPrice = discountedPrice * cartItem.Quantity; 
                }
            }

            return View(cartItems);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            var userId = GetCurrentUserId();
            var cartItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == id);
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (cartItem != null && product != null)
            {
                cartItem.Quantity += quantity;
            }
            else if (product != null)
            {
                cartItem = new CartItem
                {
                    ProductId = id,
                    Quantity = quantity,
                    UserId = userId,
                
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                return NotFound();
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var userId = GetCurrentUserId();
            var cartItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == id);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = GetCurrentUserId();
            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        private string GetCurrentUserId()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
