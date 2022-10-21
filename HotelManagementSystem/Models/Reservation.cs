using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime StartDate { get; set; }=DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public bool IsDelete { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }


    }
}