using Bogus;
using Bogus.DataSets;
using portlocator.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static portlocator.Shared.Helpers.CalculationHelper;

namespace portlocator.Tests.Shared
{
    public class SharedLibTest
    {
        [Theory]
        [InlineData(1000, 200)]
        [InlineData(3000, 100)]
        [InlineData(1891.231, 178.4655)]
        public void CalculateETA_ShouldReturn_Positive(double distanceKm, double velocityKmh)
        {
            // Arrange

            // Act
            TimeSpan eta = CalculateETA(distanceKm, velocityKmh);

            // Assert
            Assert.True(eta.TotalHours > 0);
            Assert.True(eta.TotalMinutes > 0);
        }

        [Theory]
        [InlineData(120, -30)]
        public void CalculateETA_ShouldThrow_Exception(double distanceKm, double velocityKmh)
        {
            // Arrange

            // Act
            Action test = () =>
            {
                CalculateETA(distanceKm, velocityKmh);
            };

            // Assert
            Assert.ThrowsAny<ArgumentException>(test);
        }

        [Theory]
        [InlineData(1000, 0)]
        [InlineData(0, 500)]
        public void CalculateETA_ShouldReturn_Zero(double distanceKm, double velocityKmh)
        {
            // Arrange

            // Act
            TimeSpan eta = CalculateETA(distanceKm, velocityKmh);

            // Assert
            Assert.Equal(TimeSpan.Zero, eta);
        }

        [Fact]
        public void CalculateGeoDistance_ShouldReturn_CorrectDistance()
        {
            // Arrange
            Faker<FakeShipModel> fakeShipData = new Faker<FakeShipModel>()
                .RuleFor(x => x.ShipName, f => $"Ship {f.Vehicle.Model()}")
                .RuleFor(x => x.Latitude, f => f.Address.Latitude())
                .RuleFor(x => x.Longitude, f => f.Address.Longitude());
            var fakeShip = fakeShipData.Generate(5);

            Faker<FakePortModel> fakePortData = new Faker<FakePortModel>()
                .RuleFor(x => x.PortName, f => $"Port {f.Address.City()}")
                .RuleFor(x => x.Latitude, f => f.Address.Latitude())
                .RuleFor(x => x.Longitude, f => f.Address.Longitude());
            var fakePort = fakePortData.Generate(5);

            // Act
            var shipPort = fakeShip.Zip(fakePort, (ship, port) => new
            {
                Ship = ship,
                Port = port
            }).ToList();

            // Assert
            int correctCount = 0;
            foreach (var pair in shipPort)
            {
                double distance = CalculateGeoDistance(pair.Ship.Latitude, pair.Ship.Longitude,
                                                       pair.Port.Latitude, pair.Port.Longitude);

                if (distance > 0)
                {
                    correctCount++;
                }
            }

            Assert.Equal(correctCount, shipPort.Count);
        }

        [Fact]
        public void CalculateGeoDistance_ShouldReturn_Zero()
        {
            // Arrange
            var fakeAddress = new Faker<Address>();

            Faker<FakeShipModel> fakeShipData = new Faker<FakeShipModel>()
                .RuleFor(x => x.ShipName, f => $"Ship {f.Vehicle.Model()}")
                .RuleFor(x => x.Latitude, f => f.Address.Latitude())
                .RuleFor(x => x.Longitude, f => f.Address.Longitude());
            var fakeShip = fakeShipData.Generate(5);

            var fakePort = fakeShip.Select(x => new FakePortModel
            {
                PortName = $"Port",
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList();

            // Act
            var shipPort = fakeShip.Zip(fakePort, (ship, port) => new
            {
                Ship = ship,
                Port = port
            }).ToList();

            // Assert
            int correctCount = 0;
            foreach (var pair in shipPort)
            {
                double distance = CalculateGeoDistance(pair.Ship.Latitude, pair.Ship.Longitude,
                                                       pair.Port.Latitude, pair.Port.Longitude);

                if (distance == 0)
                {
                    correctCount++;
                }
            }

            Assert.Equal(correctCount, shipPort.Count);
        }

        [Theory]
        [InlineData(180)]
        [InlineData(-180)]
        public void Radians_ShouldReturn_PiValue(double degrees)
        {
            // ACT
            double rad = Radians(degrees);

            // ASSERT
            Assert.Equal(Math.PI, Math.Abs(rad));
        }

        [Theory]
        [InlineData(0)]
        public void Radians_ShouldReturn_Zero(double degrees)
        {
            // ACT
            double rad = Radians(degrees);

            // ASSERT
            Assert.Equal(0, rad);
        }
    }
}
