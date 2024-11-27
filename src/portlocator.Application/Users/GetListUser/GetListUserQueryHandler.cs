using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Application.Abstraction.Pagination;
using portlocator.Application.Users.GetAllUser;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using portlocator.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Users.GetListUser
{
    public sealed record GetListUserQuery(PagingRequest Request) : IQuery<PagingResult<GetListUserListing>>;
    internal sealed class GetListUserQueryHandler : IQueryHandler<GetListUserQuery, PagingResult<GetListUserListing>>
    {
        private readonly AppDbContext _context;
        public GetListUserQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingResult<GetListUserListing>>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
        {
            string search = string.IsNullOrEmpty(request.Request.Search) ? string.Empty : request.Request.Search.ToLower().Trim();
            bool ignoreSearch = string.IsNullOrEmpty(search);

            var result = new PagingResult<GetListUserListing>();
            var query = _context.Users.Include(x => x.Role)
                                      .Include(x => x.ShipCrews).ThenInclude(y => y.Ship)
                                      .AsSplitQuery()
                                      .Where(x => ignoreSearch || x.Name.ToLower().Trim().Contains(search))
                                      .Select(x => new GetListUserListing
                                      {
                                          Id = x.Id,
                                          Name = x.Name,
                                          Role = x.Role.RoleName,
                                          Assignment = x.ShipCrews.Select(s => s.Ship.ShipName).ToList()
                                      });

            // CHECK ORDER
            if (!string.IsNullOrEmpty(request.Request.OrderBy))
            {
                if (request.Request.OrderDirection == OrderDirection.Descending)
                {
                    query = query.OrderByDescending(k => k.GetType().GetProperty(request.Request.OrderBy));
                }
                else
                {
                    query = query.OrderBy(k => k.GetType().GetProperty(request.Request.OrderBy));
                }
            }

            result.Count = await query.CountAsync(cancellationToken);
            result.CurrentPage = request.Request.Page;
            result.Data = await query.Paginate(request.Request.Page, request.Request.Limit)
                                     .ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
