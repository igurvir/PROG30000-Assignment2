using Microsoft.EntityFrameworkCore;
using API.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace API.Models.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
