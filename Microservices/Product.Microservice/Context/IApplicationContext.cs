using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Product.Microservice.Context
{
    public interface IApplicationContext 
    {
        DbSet<Models.Product> Products {get;set;}

        Task<int> SaveChangesAsync();
    }
}