using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Domain.Ships;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Create
{
    public sealed class CreateShipCommand : ICommand<Guid>
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }
    }

    internal sealed class CreateShipCommandHandler : ICommandHandler<CreateShipCommand, Guid>
    {
        private readonly AppDbContext _context;
        public CreateShipCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateShipCommand request, CancellationToken cancellationToken)
        {
            Ship? ship = await _context.Ships.SingleOrDefaultAsync(x => x.ShipName == request.Name, cancellationToken);
            if (ship is not null)
            {
                return Result.BadRequest(Guid.Empty, "Ship with this name already exist.");
            }

            Ship newShip = new Ship
            {
                ShipName = request.Name,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Velocity = request.Velocity
            };

            _context.Ships.Add(newShip);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(newShip.Id);
        }
    }
}
