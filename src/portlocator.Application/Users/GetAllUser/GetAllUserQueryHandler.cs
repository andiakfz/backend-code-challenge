using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Users.GetAllUser
{
    public sealed record GetAllUserQuery() : IQuery<List<GetAllUserListing>>;
    internal sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, List<GetAllUserListing>>
    {
        private readonly AppDbContext _context;
        public GetAllUserQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<GetAllUserListing>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var query =  _context.Users.Include(x => x.Role)
                                       .Include(x => x.ShipCrews).ThenInclude(y => y.Ship)
                                       .AsSplitQuery()
                                       .Select(x => new GetAllUserListing
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           Role = x.Role.RoleName,
                                           Assignment = x.ShipCrews.Select(s => s.Ship.ShipName).ToList()
                                       });

            var data = await query.ToListAsync(cancellationToken);

            return Result.Success(data);
        }
    }
}
