using EmployeeManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DAL.DBContext
{
    public class EmployeeDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("EmployeeDb");
        }
        public DbSet<Employee> Employees { get; set; }
    }
}