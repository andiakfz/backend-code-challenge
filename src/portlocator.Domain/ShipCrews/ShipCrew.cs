using portlocator.Domain.Ships;
using portlocator.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Domain.ShipCrews
{
    public class ShipCrew
    {
        public Guid UserId { get; set; }
        public Guid ShipId { get; set; }

        public User User { get; set; }
        public Ship Ship { get; set; }
    }
}
