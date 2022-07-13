using Microsoft.EntityFrameworkCore;

namespace Hubtel.eCommerce.Cart.Api.Models
{
    public class CartContext : DbContext
    {
            public CartContext(DbContextOptions<CartContext> options)
                : base(options)
            {
            }

            public DbSet<CartItems> CartItems { get; set; }
    }
}
