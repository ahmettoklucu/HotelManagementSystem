﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models.PagesModel
{
    public class UserRoleModel
    {
        public List<User> UserList { get; set; }
        public User SingleUser { get; set; }
        public List<Role> RoleList { get; set; }
        public Role SingleRole { get; set; }
    }
}