using Microsoft.EntityFrameworkCore;

namespace OdinShopping.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Models.Item> Items { get; set; } //name of table
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Cart> Carts { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.CartItem> CartItems { get; set; }
        public DbSet<Models.Payment> Payment { get; set; }
    }
}
