using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ports.Get.GetClosestPortByShipId
{
    public class PortDetails
    {
        public Guid PortId { get; set; }
        public string PortName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double ShipLatitude { get; set; }
        public double ShipLongitude { get; set; }
        public double ShipVelocity { get; set; }

        public double VelocityInKmh { get; set; }
        public double Distance { get; set; }
        public string EstimatedArrivalTime { get; set; }
    }
}
