using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Get
{
    public sealed class ShipListing
    {
        public Guid Id { get; set; }
        public string ShipName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }
    }
}
