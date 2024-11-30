using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Infrastructure.Database;
using portlocator.Shared;

namespace portlocator.Application.Users.Get.GetUsers
{
    public sealed record GetUsersQuery() : IQuery<List<UserListing>>;
    internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserListing>>
    {
        private readonly AppDbContext _context;
        public GetUsersQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<UserListing>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users.AsNoTracking()
                                      .Include(x => x.Role)
                                      .Include(x => x.ShipAssignments).ThenInclude(x => x.Ship)
                                      .AsSplitQuery()
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
                                      });

            var data = await query.ToListAsync(cancellationToken);

            return Result.Success(data);
        }
    }
}
