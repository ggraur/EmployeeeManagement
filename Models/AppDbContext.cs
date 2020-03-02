
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeeManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}


//using Microsoft.EntityFrameworkCore;

//namespace EmployeeeManagement.Models
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options)
//                : base(options)
//        {

//        }

//        public DbSet<Employee> Employees { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder) 
//        {
//            modelBuilder.Seed();
//        }
//    }
//}
