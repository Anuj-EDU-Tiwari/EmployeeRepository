using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Data
{
    public class AppDbContext:DbContext
    {
        #region constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        #endregion
        #region property
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDTO> EmployeeDTOs { get; set; }
        #endregion
        #region method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("tblEmployee1");
            modelBuilder.Entity<EmployeeDTO>().HasNoKey();
        }
        #endregion
    }
}
