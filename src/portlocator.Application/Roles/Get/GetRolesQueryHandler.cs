using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Application.Abstraction.Models;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Roles.Get
{
    public sealed record GetRolesQuery() : IQuery<List<DictionaryModel>>;
    internal sealed class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, List<DictionaryModel>>
    {
        private readonly AppDbContext _context;
        public GetRolesQueryHandler(AppDbContext context)
        {
            _context = context;    
        }

        public async Task<Result<List<DictionaryModel>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Roles.Select(x => new DictionaryModel
            {
                Key = x.Id,
                Value = x.RoleName
            });

            var result = await query.ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
