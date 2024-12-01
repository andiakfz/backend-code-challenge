using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using portlocator.Application.Users.Create.CreateUser;
using portlocator.Application.Users.Get;
using portlocator.Application.Users.Get.GetUsers;
using portlocator.Application.Users.Update.UpdateShipAssignment;
using portlocator.Infrastructure.Database;
using portlocator.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Tests.Application
{
    public class UserHandlerTest : IClassFixture<Setup>
    {
        private readonly AppDbContext _context;
        private readonly ISender _sender;
        public UserHandlerTest(Setup setup)
        {
            _context = setup.ServiceProvider.GetRequiredService<AppDbContext>();
            _sender = setup.ServiceProvider.GetRequiredService<ISender>();
        }

        [Fact]
        public async Task CreateUser_ShouldReturn_Success()
        {
            // Arrange
            var fakeUserName = FakeHelper.GenerateRandomUser();
            var roleName = FakeHelper.GetRandomSetRoles();
            var roleId = _context.Roles.First(x => x.RoleName == roleName).Id;

            // Act
            var command = new CreateUserCommand(fakeUserName.Name, roleId);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_UserExists()
        {
            // Arrange
            var roleName = FakeHelper.GetRandomSetRoles();
            var roleId = _context.Roles.First(x => x.RoleName == roleName).Id;
            var user = _context.Users.First(x => x.Name.Contains("Admin"));

            // Act
            var command = new CreateUserCommand(user.Name, roleId);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("User with this name already exist.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_RoleNotFound()
        {
            // Arrange
            var fakeUser = FakeHelper.GenerateRandomUser().Name;

            // Act
            var command = new CreateUserCommand(fakeUser, Guid.NewGuid());
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("Role with this ID not found.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_ValidationError()
        {
            // Arrange
            var fakeUser = FakeHelper.GenerateRandomUser().Name;

            // Act
            var command = new CreateUserCommand(fakeUser, Guid.Empty);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.True(result.Error.ErrorDetails.Count >= 1);
        }


        [Fact]
        public async Task GetAllUser_ShouldReturn_Success()
        {
            // Arrange

            // Act
            var query = new GetUsersQuery();
            var result = await _sender.Send(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<List<UserListing>>(result.Value);
            Assert.True(result.Value.Count >= 1);
        }

        [Fact]
        public async Task UpdateShipAssignment_ShouldReturn_Sucess()
        {
            // Arrange
            var ships = _context.Ships.AsNoTracking().Select(x => x.Id).ToList();
            var user = _context.Users.First();

            // Act
            var command = new UpdateShipAssigmentCommand(user.Id, ships);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task UpdateShipAssignment_ShouldReturnBadRequest_UserNotFound()
        {
            // Arrange
            var ships = _context.Ships.AsNoTracking().Select(x => x.Id).ToList();

            // Act
            var command = new UpdateShipAssigmentCommand(Guid.NewGuid(), ships);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Value);
            Assert.Equal(400, result.Error.ErrorCode);
            Assert.Equal("User with this ID not found.", result.Error.ErrorMessage);
        }

        [Fact]
        public async Task UpdateShipAssignment_ShouldReturnFailure_Exception()
        {
            // Arrange
            var ships = new List<Guid>() { Guid.Empty, Guid.Empty };
            var user = _context.Users.First();

            // Act
            var command = new UpdateShipAssigmentCommand(user.Id, ships);
            var result = await _sender.Send(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Value);
            Assert.Equal(500, result.Error.ErrorCode);
        }
    }
}
