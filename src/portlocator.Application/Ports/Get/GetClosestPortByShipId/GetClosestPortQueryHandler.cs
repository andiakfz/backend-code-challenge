﻿using Microsoft.EntityFrameworkCore;
using portlocator.Application.Abstraction.Messaging;
using portlocator.Domain.Ships;
using portlocator.Infrastructure.Database;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static portlocator.Shared.Helpers.CalculationHelper;

namespace portlocator.Application.Ports.Get.GetClosestPortByShipId
{
    public sealed record GetClosestPortQuery(Guid ShipId) : ICommand<PortDetails>;
    internal sealed class GetClosestPortQueryHandler : ICommandHandler<GetClosestPortQuery, PortDetails>
    {
        private readonly AppDbContext _context;
        public GetClosestPortQueryHandler(AppDbContext context)
        {
            _context = context;    
        }

        public async Task<Result<PortDetails>> Handle(GetClosestPortQuery request, CancellationToken cancellationToken)
        {
            Ship? ship = await _context.Ships.SingleOrDefaultAsync(x => x.Id == request.ShipId, cancellationToken);
            if (ship is null)
            {
                return Result.BadRequest<PortDetails>(null, "Ship with this ID not found.");
            }

            var portDetail = _context.Ports.AsNoTracking()
                                           .Select(x => new PortDetails
                                           {
                                               PortId = x.Id,
                                               PortName = x.PortName,
                                               Latitude = x.Latitude,
                                               Longitude = x.Longitude,
                                               ShipLatitude = ship.Latitude,
                                               ShipLongitude = ship.Longitude,
                                               ShipVelocity = ship.Velocity,
                                           
                                               VelocityInKmh = VelocityToKmh(ship.Velocity),
                                               Distance = CalculateGeoDistance(ship.Latitude, ship.Longitude, x.Latitude, x.Longitude)
                                           })
                                           .OrderBy(x => x.Distance)
                                           .First();

            var etaTime = CalculateETA(portDetail.Distance, portDetail.VelocityInKmh);
            portDetail.EstimatedArrivalTime = $"{etaTime.Hours} Hour and {etaTime.Minutes} Minutes";

            return Result.Success(portDetail);
        }
    }
}