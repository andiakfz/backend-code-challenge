using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Domain.Roles;
using portlocator.Domain.ShipAssignments;
using portlocator.Domain.Users;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Users.Create.CreateUser
{
    public sealed record CreateUserCommand(string Name, Guid RoleId) : ICommand<Guid>;
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly AppDbContext _context;
        public CreateUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(x => x.Name.ToLower().Trim() == request.Name.ToLower().Trim(), cancellationToken);
            Role? role = await _context.Roles.SingleOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);

            if (user is not null)
            {
                return Result.BadRequest(Guid.Empty, "User with this name already exist.");
            }
            if (role is null)
            {
                return Result.BadRequest(Guid.Empty, "Role with this ID not found.");
            }

            User newUser = new()
            {
                Name = request.Name,
                RoleId = request.RoleId
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(newUser.Id);
        }
    }
}
