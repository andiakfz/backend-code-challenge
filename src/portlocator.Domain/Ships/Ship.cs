using portlocator.Domain.ShipCrews;
using portlocator.Domain.Users;

namespace portlocator.Domain.Ships
{
    public class Ship
    {
        public Guid Id { get; set; }
        public string ShipName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }

        public List<ShipCrew> ShipCrews { get; set; }
    }
}
