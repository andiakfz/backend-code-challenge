using MediatR;
using Microsoft.AspNetCore.Mvc;
using portlocator.Application.Abstraction.Pagination;
using portlocator.Application.Users.Create.CreateUser;
using portlocator.Application.Users.Get;
using portlocator.Application.Users.Get.GetUsers;
using portlocator.Application.Users.Get.ListUsers;
using portlocator.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace portlocator.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get All User",
            Description = "Get All User in the System"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<UserListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<UserListing>>))]
        public async Task<IResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetUsersQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result);
        }

        [HttpGet]
        [Route("List")]
        [SwaggerOperation(
            Summary = "Get List of User",
            Description = "Get List of User with Pagination"
        )]
        [SwaggerResponse(200, Type = typeof(Result<PagingResult<UserListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<PagingResult<UserListing>>))]
        public async Task<IResult> List(
            CancellationToken cancellationToken,
            int page = 1,
            int rows = 10,
            string search = "",
            string orderBy = "",
            OrderDirection order = OrderDirection.Asc)
        {
            var request = new PagingRequest
            {
                Search = search,
                Page = page,
                Limit = rows,
                OrderBy = orderBy,
                OrderDirection = order
            };

            var query = new ListUserQuery(request);

            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create User",
            Description = "Create a new User using Name and Role"
        )]
        [SwaggerResponse(200, Type = typeof(Result<Guid>))]
        [SwaggerResponse(400, Type = typeof(Result<Guid>))]
        [SwaggerResponse(500, Type = typeof(Result<Guid>))]
        public async Task<IResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        }
    }
}
