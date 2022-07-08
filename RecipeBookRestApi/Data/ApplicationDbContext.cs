using Microsoft.EntityFrameworkCore;
using RecipeBookRestApi.Models;

namespace RecipeBookRestApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
                
        }
    }
}
