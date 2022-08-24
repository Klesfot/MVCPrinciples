using Microsoft.EntityFrameworkCore;

namespace MVCPrinciples.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.Product> Products { get; set; }
    }
}