using Microsoft.EntityFrameworkCore;
using WorkingWithMultipleTable_Prod.Models;

namespace WorkingWithMultipleTable_Prod.Data
{
    public class ApplicationContext :DbContext
    {
       public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }
       
       public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
