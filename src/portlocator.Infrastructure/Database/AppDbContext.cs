using Microsoft.EntityFrameworkCore;
using portlocator.Domain.Ports;
using portlocator.Domain.Roles;
using portlocator.Domain.ShipAssignments;
using portlocator.Domain.Ships;
using portlocator.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<ShipAssignment> ShipAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
