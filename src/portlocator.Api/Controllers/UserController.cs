using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using portlocator.Application.Abstraction.Models;
using portlocator.Application.Abstraction.Pagination;
using portlocator.Application.Users.GetAllUser;
using portlocator.Application.Users.GetListUser;
using portlocator.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace portlocator.Api.Controllers
{
    [ApiController]
    [Route("api/User")]
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
        [SwaggerResponse(200, Type = typeof(Result<List<GetAllUserListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<GetAllUserListing>>))]
        public async Task<IResult> Get(CancellationToken cancellationToken)
        {
            var command = new GetAllUserQuery();

            var result = await _sender.Send(command, cancellationToken);

            return Results.Ok(result);
        }

        [HttpGet]
        [Route("List")]
        [SwaggerOperation(
            Summary = "Get List of User",
            Description = "Get List of User with Pagination"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<GetListUserListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<GetListUserListing>>))]
        public async Task<IResult> List(
            CancellationToken cancellationToken,
            int page = 1,
            int rows = 10,
            string search = "",
            string orderBy = "",
            OrderDirection order = OrderDirection.Ascending)
        {
            var request = new PagingRequest
            {
                Search = search,
                Page = page,
                Limit = rows,
                OrderBy = orderBy,
                OrderDirection = order
            };

            var command = new GetListUserQuery(request);

            var result = await _sender.Send(command, cancellationToken);

            return Results.Ok(result);
        }


    }
}
