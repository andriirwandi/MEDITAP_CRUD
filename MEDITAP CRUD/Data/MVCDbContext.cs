using MEDITAP_CRUD.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MEDITAP_CRUD.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options )
        {

        }

        // Setup DBContext berdasarkan model untuk retrieve parameter ke table Employees
        public DbSet<Employee> Employees { get; set; }

    }
}
