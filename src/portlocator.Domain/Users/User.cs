using portlocator.Domain.Roles;
using portlocator.Domain.ShipAssignments;
using portlocator.Domain.Ships;

namespace portlocator.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Name { get; set; }

        public Role Role { get; set; }
        public List<ShipAssignment> ShipAssignments { get; set; }
    }
}
