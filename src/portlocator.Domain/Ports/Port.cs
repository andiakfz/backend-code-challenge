using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Domain.Ports
{
    public class Port
    {
        public Guid Id { get; set; }
        public string PortName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
