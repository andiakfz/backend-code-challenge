using MediatR;
using Microsoft.AspNetCore.Mvc;
using portlocator.Application.Ports.Get.GetAllPorts;
using portlocator.Application.Ports.Get.GetClosestPortByShipId;
using portlocator.Application.Users.Get;
using portlocator.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace portlocator.Api.Controllers
{
    [ApiController]
    [Route("api/port")]
    public class PortController : ControllerBase
    {
        private readonly ISender _sender;
        public PortController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get All Ports",
            Description = "Get All Ports in the System"
        )]
        [SwaggerResponse(200, Type = typeof(Result<List<PortListing>>))]
        [SwaggerResponse(500, Type = typeof(Result<List<PortListing>>))]
        public async Task<IResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetAllPortsQuery();
            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result);
        }

        [HttpGet]
        [Route("closest/{shipId}")]
        [SwaggerOperation(
            Summary = "Get Closest Port",
            Description = "Get the closest port based on ship location"
        )]
        [SwaggerResponse(200, Type = typeof(Result<PortDetails>))]
        [SwaggerResponse(400, Type = typeof(Result<PortDetails>))]
        [SwaggerResponse(500, Type = typeof(Result<PortDetails>))]
        public async Task<IResult> GetClosestPort(Guid shipId,CancellationToken cancellationToken)
        {
            var query = new GetClosestPortQuery(shipId);
            var result = await _sender.Send(query, cancellationToken);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        }
    }
}
