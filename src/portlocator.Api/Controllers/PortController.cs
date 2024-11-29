using MediatR;
using Microsoft.AspNetCore.Mvc;
using portlocator.Application.Ports.Get.GetAllPorts;
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
    }
}
