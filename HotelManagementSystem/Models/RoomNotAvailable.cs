using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class RoomNotAvailable
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public DateTime Date { get; set; }
        public bool IsDelete { get; set; }
        public int ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation reservation { get; set; }
    }
}