using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace EmployeeeManagement.Models
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mary",
                    Department = Dept.HR,
                    Email = "mary@hotmail.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Albert",
                    Department = Dept.HR,
                    Email = "albert@hotmail.com"
                }
                );
        }
    }
}
