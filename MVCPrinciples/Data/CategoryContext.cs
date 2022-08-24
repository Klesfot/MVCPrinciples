using Microsoft.EntityFrameworkCore;

namespace MVCPrinciples.Data
{
    public class CategoryContext : DbContext
    {
        public CategoryContext(DbContextOptions<CategoryContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Models.Category> Categories { get; set; }  
    }
}
