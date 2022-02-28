using Microsoft.EntityFrameworkCore;

namespace CartApi.Models
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options): base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; } = null!;
    }
}
