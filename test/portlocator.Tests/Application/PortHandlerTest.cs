using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using portlocator.Application.Ports.Get.GetAllPorts;
using portlocator.Application.Ports.Get.GetClosestPortByShipId;
using portlocator.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Tests.Application
{
    public class PortHandlerTest : IClassFixture<Setup>
    {
        private readonly AppDbContext _context;
        private readonly ISender _sender;
        public PortHandlerTest(Setup setup)
        {
            _context = setup.ServiceProvider.GetRequiredService<AppDbContext>();
            _sender = setup.ServiceProvider.GetRequiredService<ISender>();
        }

        [Fact]
        public async Task GetAllPort_ShouldReturn_Success()
        {
            // Arrange
            var query = new GetAllPortsQuery();

            // Act
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<List<PortListing>>(result.Value);
            Assert.True(result.Value.Count >= 1);
        }


        [Fact]
        public async Task GetClosestPort_ShouldReturnSuccess_NormalCase()
        {
            // Arrange
            var ship = _context.Ships.Where(x => x.Velocity >= 0).First();
            var query = new GetClosestPortQuery(ship.Id);

            // Act
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<PortDetails>(result.Value);
            Assert.NotEmpty(result.Value.EstimatedArrivalTime);
            Assert.True(result.Value.Distance >= 0);
        }

        [Fact]
        public async Task GetClosestPort_ShouldReturnSuccess_EtaUndetermined()
        {
            // Arrange
            var ship = await _context.Ships.Where(x => x.Velocity == 0).FirstOrDefaultAsync();
            if (ship is null)
            {
                ship = new Domain.Ships.Ship
                {
                    ShipName = "Zero Velocity Ship",
                    Latitude = 90,
                    Longitude = 180,
                    Velocity = 0
                };

                _context.Ships.Add(ship);
                _context.SaveChanges();
            }
            var query = new GetClosestPortQuery(ship.Id);

            // Act
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<PortDetails>(result.Value);
            Assert.Equal("Could not be determined due to Zero ship velocity.", result.Value.EstimatedArrivalTime);
        }

        [Fact]
        public async Task GetClosestPort_ShouldReturnBadRequest_ShipNotFound()
        {
            // Arrange
            var query = new GetClosestPortQuery(Guid.NewGuid());

            // Act
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("Ship with this ID not found.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task GetClosestPort_ShouldReturnFailure_Exception()
        {
            // Arrange
            var ship = await _context.Ships.Where(x => x.Velocity < 0).FirstOrDefaultAsync();
            if (ship is null)
            {
                ship = new Domain.Ships.Ship
                {
                    ShipName = "Negative Velocity Ship",
                    Latitude = 90,
                    Longitude = 180,
                    Velocity = -15
                };

                _context.Ships.Add(ship);
                _context.SaveChanges();
            }

            var query = new GetClosestPortQuery(ship.Id);

            // Act
            Func<Task> queryCall = async () =>
            {
                await _sender.Send(query, CancellationToken.None);
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(queryCall);
        }
    }
}
