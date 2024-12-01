using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Application.Abstraction.Pagination;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Get.GetShips
{
    public sealed record GetShipsQuery() : IQuery<List<ShipListing>>;
    public sealed class GetShipsQueryHandler : IQueryHandler<GetShipsQuery, List<ShipListing>>
    {
        private readonly AppDbContext _context;
        public GetShipsQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ShipListing>>> Handle(GetShipsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Ships.AsNoTracking()
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
