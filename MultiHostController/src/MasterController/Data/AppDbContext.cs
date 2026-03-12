using Microsoft.EntityFrameworkCore;
using MasterController.Models;

namespace MasterController.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<MasterController.Models.Host> Hosts { get; set; }

        public DbSet<DeploymentTask> Tasks { get; set; }
    }
}