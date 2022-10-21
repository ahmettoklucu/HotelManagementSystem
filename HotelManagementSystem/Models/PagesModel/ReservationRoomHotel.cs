using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models.PagesModel
{
    public class ReservationRoomHotel
    {
        public Reservation SingleReservation { get; set; }
        public Room Room { get; set; }
        public List<Reservation> ReservationList { get; set; }
        public List<Room> RoomList { get; set; }
        public List<Hotel> HotelList { get; set; }
        public List<RoomNotAvailable> RoomNotAvailableList { get; set; }
        public List<User> UserList { get; set; }
    }
}