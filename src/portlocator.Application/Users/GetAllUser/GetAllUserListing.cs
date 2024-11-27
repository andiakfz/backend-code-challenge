﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Users.GetAllUser
{
    public sealed class GetAllUserListing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public List<string> Assignment { get; set; }
    }
}