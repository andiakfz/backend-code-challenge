using Bogus;
using Microsoft.EntityFrameworkCore;
using portlocator.Domain.Ports;
using portlocator.Domain.Roles;
using portlocator.Domain.Ships;
using portlocator.Domain.Users;
using portlocator.Infrastructure.Database;
using portlocator.Infrastructure.Seeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Tests
{
    public class TestSeeder
    {
        private readonly AppDbContext _context;
        public TestSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedRole();
            await SeedPorts();

            await SeedUser();
            await SeedShip();

            await SetAssignment();
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

            var fakes = new Faker<SeedPortModel>()
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

        private async Task SeedUser()
        {
            if (_context.Users.Any())
            {
                return;
            }

            Role superAdmin = _context.Roles.AsNoTracking().First(x => x.RoleName == "Super Admin");
            Role admin = _context.Roles.AsNoTracking().First(x => x.RoleName == "Admin");
            User user = new()
            {
                Name = "Admin Tester",
                RoleId = superAdmin.Id
            };

            User user2 = new()
            {
                Name = "Random User",
                RoleId = admin.Id
            };

            _context.Users.AddRange(user, user2);
            await _context.SaveChangesAsync();
        }

        private async Task SeedShip()
        {
            if (_context.Ships.Any())
            {
                return;
            }

            Ship ship = new()
            {
                ShipName = "Ship Administrator",
                Latitude = 86.721,
                Longitude = 122.1134,
                Velocity = 300
            };

            _context.Ships.Add(ship);
            await _context.SaveChangesAsync();
        }

        private async Task SetAssignment()
        {
            if (_context.ShipAssignments.Any())
            {
                return;
            }

            var userId = _context.Users.AsNoTracking().Single(x => x.Name == "Admin Tester").Id;
            var shipId = _context.Ships.AsNoTracking().Single(x => x.ShipName == "Ship Administrator").Id;

            _context.ShipAssignments.Add(new Domain.ShipAssignments.ShipAssignment
            {
                ShipId = shipId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }
    }
}
