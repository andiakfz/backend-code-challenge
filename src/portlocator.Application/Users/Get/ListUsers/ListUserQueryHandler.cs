using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Application.Abstraction.Pagination;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using portlocator.Shared.Extensions;

namespace portlocator.Application.Users.Get.ListUsers
{
    public sealed record ListUserQuery(PagingRequest Paging) : IQuery<PagingResult<UserListing>>;
    internal sealed class ListUserQueryHandler : IQueryHandler<ListUserQuery, PagingResult<UserListing>>
    {
        private readonly AppDbContext _context;
        public ListUserQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingResult<UserListing>>> Handle(ListUserQuery request, CancellationToken cancellationToken)
        {
            string search = string.IsNullOrEmpty(request.Paging.Search) ? string.Empty : request.Paging.Search.ToLower().Trim();
            bool ignoreSearch = string.IsNullOrEmpty(search);

            var result = new PagingResult<UserListing>();
            var query = _context.Users.AsNoTracking()
                                      .Include(x => x.Role)
                                      .Include(x => x.ShipAssignments).ThenInclude(y => y.Ship)
                                      .AsSplitQuery()
                                      .Where(x => ignoreSearch || x.Name.ToLower().Trim().Contains(search))
                                      .Select(x => new UserListing
                                      {
                                          Id = x.Id,
                                          Name = x.Name,
                                          Role = x.Role.RoleName,
                                          AssignedTo = x.ShipAssignments.Select(x => new UserAssignmentListing
                                          {
                                              ShipId = x.ShipId,
                                              ShipName = x.Ship.ShipName
                                          }).ToList()
                                      })
                                      .OrderBy(request.Paging.OrderBy, request.Paging.OrderDirection == OrderDirection.Desc);

            result.Count = await query.CountAsync(cancellationToken);
            result.CurrentPage = request.Paging.Page;
            result.Data = await query.Paginate(request.Paging.Page, request.Paging.Limit)
                                     .ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
