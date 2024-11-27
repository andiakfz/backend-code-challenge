using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using portlocator.Application.Abstraction.Models;
using portlocator.Application.Roles.Get;
using portlocator.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace portlocator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ISender _sender;
        public RoleController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get Roles",
            Description = "Get All Roles with its ID and Role Name"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<DictionaryModel>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<DictionaryModel>>))]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<DictionaryModel>>))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<List<DictionaryModel>>))]
        public async Task<IResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetRolesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result);
        }
    }
}
