using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public List<Room> Room { get; set; }
    }
}