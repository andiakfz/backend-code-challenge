using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Get.GetUnassignedShips
{
    public sealed record GetUnassignedShipsQuery() : IQuery<List<ShipListing>>;
    internal sealed class GetUnassignedShipsQueryHandler : IQueryHandler<GetUnassignedShipsQuery, List<ShipListing>>
    {
        private readonly AppDbContext _context;
        public GetUnassignedShipsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ShipListing>>> Handle(GetUnassignedShipsQuery request, CancellationToken cancellationToken)
        {
            var query =  _context.Ships.AsNoTracking()
                                       .Include(x => x.ShipAssignments)
                                       .Where(x => x.ShipAssignments.Count < 1)
                                       .Select(x => new ShipListing
                                       {
                                           Id = x.Id,
                                           ShipName = x.ShipName,
                                           Latitude = x.Latitude,
                                           Longitude = x.Longitude,
                                           Velocity = x.Velocity
                                       });

            var result = await query.ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
