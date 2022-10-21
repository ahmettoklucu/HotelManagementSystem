using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models.PagesModel
{
    public class HotelUserModel
    {
        public Hotel SingleHotel { get; set; }
        public List<Hotel> HotelList { get; set; }
        public List<User> UserList { get; set; }
    }
}