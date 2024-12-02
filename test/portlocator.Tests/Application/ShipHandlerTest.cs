using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using portlocator.Application.Ships.Create;
using portlocator.Application.Ships.Get;
using portlocator.Application.Ships.Get.GetShips;
using portlocator.Application.Ships.Get.GetShipsByUserId;
using portlocator.Application.Ships.Get.GetUnassignedShips;
using portlocator.Application.Ships.Update.UpdateVelocity;
using portlocator.Domain.Ships;
using portlocator.Infrastructure.Database;
using portlocator.Tests.Fakes;

namespace portlocator.Tests.Application
{
    public class ShipHandlerTest : IClassFixture<Setup>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateShipVelocityCommandHandler> _logger;
        private readonly ISender _sender;

        public ShipHandlerTest(Setup setup)
        {
            _context = setup.ServiceProvider.GetRequiredService<AppDbContext>();
            _sender = setup.ServiceProvider.GetRequiredService<ISender>();
            _logger = setup.ServiceProvider.GetRequiredService<ILogger<UpdateShipVelocityCommandHandler>>();
        }

        [Fact]
        public async Task CreateShip_ShouldReturn_Success()
        {
            // Arrange
            var fakeShip = FakeHelper.GenerateRandomShip();
            var command = new CreateShipCommand()
            {
                Name = fakeShip.ShipName,
                Latitude = fakeShip.Latitude,
                Longitude = fakeShip.Longitude,
                Velocity = fakeShip.Velocity
            };

            // Act
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

        [Fact]
        public async Task CreateShip_ShouldReturnBadRequest_ShipExist()
        {
            // Arrange
            var command = new CreateShipCommand()
            {
                Name = "Ship Administrator",
                Latitude = 86.721,
                Longitude = 122.1134,
                Velocity = 300
            };

            // Act
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("Ship with this name already exist.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task CreateShip_ShouldReturnBadRequest_ValidationError()
        {
            // Arrange
            var command = new CreateShipCommand()
            {
                Name = "Random Ship",
                Latitude = -95.14045,
                Longitude = 182.39019,
                Velocity = -12
            };

            // Act
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.True(result.Error.ErrorDetails.Count >= 1);
        }


        [Fact]
        public async Task GetShipAll_ShouldReturn_Success()
        {
            // Arrange
            var ships = _context.Ships;
            if (!ships.Any())
            {
                var fakeShip = FakeHelper.GenerateRandomShip();
                _context.Ships.Add(new Ship
                {
                    ShipName = fakeShip.ShipName,
                    Latitude = fakeShip.Latitude,
                    Longitude = fakeShip.Longitude,
                    Velocity = fakeShip.Velocity
                });
                _context.SaveChanges();
            }

            // Act
            var query = new GetShipsQuery();
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<List<ShipListing>>(result.Value);
            Assert.True(result.Value.Count >= 1);
        }

        [Fact]
        public async Task GetShipByUserId_ShouldReturn_Success()
        {
            // Arrange
            var assignment = _context.ShipAssignments.First();

            // Act
            var query = new GetShipsByUserIdQuery(assignment.UserId);
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<List<ShipListing>>(result.Value);
            Assert.True(result.Value.Count >= 1);
        }

        [Fact]
        public async Task GetShipByUserId_ShouldReturnBadRequest_UserNotFound()
        {
            // Arrange

            // Act
            var query = new GetShipsByUserIdQuery(Guid.NewGuid());
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("User with this ID not found.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task GetShipByUserId_ShouldReturnBadRequest_NoShipAssignment()
        {
            // Arrange
            var user = _context.Users.AsNoTracking()
                                     .Include(x => x.ShipAssignments)
                                     .Where(x => x.ShipAssignments.Count == 0)
                                     .Select(x => x.Id)
                                     .First();

            // Act
            var query = new GetShipsByUserIdQuery(user);
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("This user does not have any ships assigned.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task GetShipUnassigned_ShouldReturn_Success()
        {
            // Arrange

            // Act
            var query = new GetUnassignedShipsQuery();
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<List<ShipListing>>(result.Value);
            Assert.True(result.Value.Count >= 1);
        }

        [Fact]
        public async Task UpdateShipVelocity_ShouldReturn_Success()
        {
            // Arrange
            double velocity = 350.12;
            var shipId = _context.Ships.First().Id;

            // Act
            var command = new UpdateShipVelocityCommand(shipId, velocity);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
    }
}
