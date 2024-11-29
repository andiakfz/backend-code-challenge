﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Users.Get
{
    public sealed class UserListing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public List<UserCrewListing> AssignedTo { get; set; }
    }

    public sealed class UserCrewListing
    {
        public Guid ShipId { get; set; }
        public string ShipName { get; set; }
    }
}