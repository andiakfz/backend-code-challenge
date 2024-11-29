using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using portlocator.Application.Abstraction.Models;
using portlocator.Application.Roles.Get;
using portlocator.Application.Ships.Create;
using portlocator.Application.Ships.Get;
using portlocator.Application.Ships.Get.GetShips;
using portlocator.Application.Ships.Get.GetShipsByUserId;
using portlocator.Application.Ships.Get.GetUnassignedShips;
using portlocator.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace portlocator.Api.Controllers
{
    [ApiController]
    [Route("api/ship")]
    public class ShipController : ControllerBase
    {
        private readonly ISender _sender;
        public ShipController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get All Ships",
            Description = "Get All Ships in the System"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<ShipListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<ShipListing>>))]
        public async Task<IResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetShipsQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result);
        }

        [HttpGet]
        [Route("{userId}")]
        [SwaggerOperation(
            Summary = "Get Ships from User",
            Description = "Get Ships assigned to a Specific User"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<ShipListing>>))]
        [SwaggerResponse(400, Type = typeof(Result<List<ShipListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<ShipListing>>))]
        public async Task<IResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetShipsByUserIdQuery(userId);

            var result = await _sender.Send(query, cancellationToken);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        }

        [HttpGet]
        [Route("unassigned")]
        [SwaggerOperation(
            Summary = "Get Unassigned Ships",
            Description = "Get Ships with no user assigned to the Ship"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<ShipListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<ShipListing>>))]
        public async Task<IResult> GetUnassignedShips(CancellationToken cancellationToken)
        {
            var query = new GetUnassignedShipsQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create Ship",
            Description = "Create a new Ship data"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<ShipListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<ShipListing>>))]
        public async Task<IResult> Create(CreateShipCommand command, CancellationToken cancellationToken)
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
