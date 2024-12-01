using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Tests.Fakes
{
    public static class FakeHelper
    {
        public static FakeShipModel GenerateRandomShip()
        {
            var faker = new Faker<FakeShipModel>()
                .RuleFor(x => x.ShipName, f => $"Ship {f.Vehicle.Model()}")
                .RuleFor(x => x.Latitude, f => f.Address.Latitude())
                .RuleFor(x => x.Longitude, f => f.Address.Longitude())
                .RuleFor(x => x.Velocity, f => f.Random.Double());

            var generate = faker.Generate(1);

            return generate[0];
        }

        public static FakeUserModel GenerateRandomUser()
        {
            var faker = new Faker<FakeUserModel>()
                .RuleFor(x => x.Name, f => f.Name.FullName());
            var generate = faker.Generate(1);

            return generate[0];
        }

        public static string GetRandomSetRoles()
        {
            var listRoles = new List<string> { "Super Admin", "Admin", "Captain", "Chef", "Sailor", "Guest" };
            var role = listRoles.OrderBy(s => Guid.NewGuid()).First();

            return role;
        }
    }
}
