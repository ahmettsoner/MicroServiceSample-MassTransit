using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Product.Microservice.Context
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public DbSet<Models.Product> Products {get;set;}

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public async Task<int> SaveChangesAsync(){
            return await base.SaveChangesAsync();
        }
    }
}