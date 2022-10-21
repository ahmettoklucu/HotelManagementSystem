using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string RoomName { get; set; }
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        public int RoomTypeId { get; set; }
        [ForeignKey("RoomTypeId")]
        public RoomType RoomType { get; set; }
        public decimal PeerDeim { get; set; }
        public bool IsDelete { get; set; }
        public bool Status { get; set; }
        public List<Reservation> ReservationLists { get; set; }
        public List<RoomNotAvailable> RoomNotAvailable { get; set; }
    }
}