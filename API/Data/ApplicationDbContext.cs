using API.Enums;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .ToTable("order")
                .Property(e => e.OrderType)
                .HasConversion<int>();

            modelBuilder.Entity<User>()
                .ToTable("user");
        }

        public DbSet<API.Models.User> User { get; set; }
    }
}
