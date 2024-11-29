using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Domain.Users;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Get.GetShipsByUserId
{
    public sealed record GetShipsByUserIdQuery(Guid UserId) : IQuery<List<ShipListing>>;
    internal sealed class GetShipsByUserIdQueryHandler : IQueryHandler<GetShipsByUserIdQuery, List<ShipListing>>
    {
        private readonly AppDbContext _context;
        public GetShipsByUserIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ShipListing>>> Handle(GetShipsByUserIdQuery request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return Result.BadRequest<List<ShipListing>>(null, "User with this ID not found.");
            }

            var shipCrew = _context.ShipAssignments.Where(x => x.UserId == request.UserId);
            if (!shipCrew.Any())
            {
                return Result.BadRequest<List<ShipListing>>(null, "This user does not have any ships assigned.");
            }

            var shipListing = shipCrew.AsNoTracking()
                                      .Include(x => x.Ship)
                                      .Select(x => new ShipListing
                                      {
                                          Id = x.ShipId,
                                          ShipName = x.Ship.ShipName,
                                          Latitude = x.Ship.Latitude,
                                          Longitude = x.Ship.Longitude,
                                          Velocity = x.Ship.Velocity
                                      });

            var result = await shipListing.ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
