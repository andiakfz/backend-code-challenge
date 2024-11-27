using Bogus;
using portlocator.Domain.Ports;
using portlocator.Domain.Roles;
using portlocator.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure.Seeder
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedRole();
            await SeedPorts();
        }

        private async Task SeedRole()
        {
            if (_context.Roles.Any())
            {
                return;
            }

            var defaultRoles = new List<string> { "Super Admin", "Admin", "Captain", "Chef", "Sailor", "Guest" };
            var roles = defaultRoles.Select(x => new Role
            {
                RoleName = x
            }).ToList();

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync();
        }

        private async Task SeedPorts()
        {
            if (_context.Ports.Any())
            {
                return;
            }

            var fakes = new Faker<FakePortModel>()
                .RuleFor(x => x.PortName, f => $"Port {f.Address.City()}")
                .RuleFor(x => x.Latitude, f => f.Address.Latitude())
                .RuleFor(x => x.Longitude, f => f.Address.Longitude());

            var fakePorts = fakes.Generate(10);

            var ports = fakePorts.Select(x => new Port
            {
                PortName = x.PortName,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList();

            _context.Ports.AddRange(ports);
            await _context.SaveChangesAsync();
        }
    }
}
