using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models.PagesModel
{
    public class RoomHotelRoomType
    {
        public Hotel SingleHotel { get; set; }
        public Room SingleRoom { get; set; }
        public List<Hotel> HotelList { get; set; }
        public  List<RoomType> RoomTypeList  { get; set; }
        public List<Room> RoomList { get; set; }
        public List<User> UserList { get; set; }
    }
}