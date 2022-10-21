using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(25)]
        public string Name { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [StringLength(500)]
        public string Adress { get; set; }
        [StringLength(30)]
        public string CityName { get; set; }
        public bool IsDelete { get; set; }
        public string Image { get; set; }
        public string WepSite { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public List<Room> Room { get; set; }
        public List<Employee> Employee { get; set; }


    }
}