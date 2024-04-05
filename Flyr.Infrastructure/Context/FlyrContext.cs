using Flyr.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace FlySupermarket.Infrastructure.Context
{
    public class FlyrContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public FlyrContext(DbContextOptions options) : base(options)
        {
            LoadProducts();
            SaveChanges();
        }

        public void LoadProducts()
        {
            Product product = new()
            {
                Code = "GR1",
                Name = "Green tea",
                Price = 3.11M
            };
            Products.Add(product);
            product = new()
            {
                Code = "SR1",
                Name = "Strawberries",
                Price = 5.00M
            };
            Products.Add(product);
            product = new()
            {
                Code = "CF1",
                Name = "Coffee",
                Price = 11.23M
            };
            Products.Add(product);
        }

        public List<Product> GetProducts()
        {
            return [.. Products];
        }
    }
}
