using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Domain.Ships;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Update.UpdateVelocity
{
    public sealed record UpdateShipVelocityCommand(Guid ShipId, double Velocity) : ICommand<bool>;
    internal sealed class UpdateShipVelocityCommandHandler : ICommandHandler<UpdateShipVelocityCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateShipVelocityCommandHandler> _logger;
        public UpdateShipVelocityCommandHandler(
            AppDbContext context,
            ILogger<UpdateShipVelocityCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(UpdateShipVelocityCommand request, CancellationToken cancellationToken)
        {
            Ship? ship = await _context.Ships.SingleOrDefaultAsync(x => x.Id == request.ShipId, cancellationToken);
            if (ship is null)
            {
                return Result.BadRequest(false, "Ship with this ID not found.");
            }

            var transaction = _context.Database.BeginTransaction();
            try
            {
                ship.Velocity = request.Velocity;

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

                return Result.Failure(false, "Failed to update due to unexpected error.");
            }
        }
    }
}
