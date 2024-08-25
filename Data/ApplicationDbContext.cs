using Microsoft.EntityFrameworkCore;
using TopSpeedAutomobile.Models;

namespace TopSpeedAutomobile.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        
        }
        public DbSet<Brand> Brand { get; set; }
    }
}
