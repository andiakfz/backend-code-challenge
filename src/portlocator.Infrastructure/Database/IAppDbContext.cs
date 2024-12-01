using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

namespace portlocator.Infrastructure
{
    public interface IAppDbContext
    {
        DatabaseFacade Database { get; }

        public DbSet<User> Users { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<Ship> Ships { get; }
        public DbSet<Port> Ports { get; }
        public DbSet<ShipAssignment> ShipAssignments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
