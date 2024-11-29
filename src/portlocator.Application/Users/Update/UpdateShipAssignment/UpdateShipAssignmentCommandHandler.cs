using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Domain.ShipAssignments;
using portlocator.Domain.Users;
using portlocator.Infrastructure.Database;
using portlocator.Shared;

namespace portlocator.Application.Users.Update.UpdateShipAssignment
{
    public sealed record UpdateShipAssigmentCommand(Guid UserId, List<Guid> Ships) : ICommand<bool>;
    internal sealed class UpdateShipAssignmentCommandHandler : ICommandHandler<UpdateShipAssigmentCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateShipAssignmentCommandHandler> _logger;
        public UpdateShipAssignmentCommandHandler(
            AppDbContext context, 
            ILogger<UpdateShipAssignmentCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(UpdateShipAssigmentCommand request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return Result.BadRequest(false, "User with this ID not found.");
            }

            var transaction = _context.Database.BeginTransaction();
            try
            {
                // REMOVE EXISTING ASSIGNMENT
                var currentAssignment = await _context.ShipAssignments.Where(x => x.UserId == request.UserId).ToListAsync(cancellationToken);
                _context.ShipAssignments.RemoveRange(currentAssignment);

                // INSERT NEW ASSIGNMENT
                var newAssignment = request.Ships.Select(shipId => new ShipAssignment
                {
                    ShipId = shipId,
                    UserId = request.UserId
                }).ToList();

                _context.ShipAssignments.AddRange(newAssignment);
                await _context.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return Result.Success(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Error Updating Data => {Message}", ex.Message);

                transaction.Rollback();

                return Result.Failure(false, "Failed to update due to database error.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating Data => {Message}", ex.Message);

                transaction.Rollback();

                return Result.Failure(false, "Failed to update due to server error.");
            }
        }
    }
}
