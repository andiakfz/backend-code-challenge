using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ports.Get.GetAllPorts
{
    public sealed record GetAllPortsQuery() : IQuery<List<PortListing>>;
    internal sealed class GetAllPortsQueryHandler : IQueryHandler<GetAllPortsQuery, List<PortListing>>
    {
        private readonly AppDbContext _context;
        public GetAllPortsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<PortListing>>> Handle(GetAllPortsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Ports.Select(x => new PortListing
            {
                Id = x.Id,
                PortName = x.PortName,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });

            var result = await query.ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
