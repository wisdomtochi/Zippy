using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Zippy.Entities;

namespace Zippy.Data.Context
{
    public class ZippyDbContext : DbContext
    {
        public ZippyDbContext(DbContextOptions<ZippyDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Resource>().HasData(new Resource
            {
                Id = Guid.NewGuid(),
                Url = "https://github.com/wisdomtochi",
                Key = "resource1-key",
                Name = "Resource 1",
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
