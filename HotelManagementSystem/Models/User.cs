using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(25)]
        public string UserName { get; set; }
        [StringLength(25)]
        public string Password { get; set; }
        [StringLength(25)]
        public string Name { get; set; }
        [StringLength(25)]
        public string Surename { get; set; }
        [StringLength(40)]
        public string CityName { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [StringLength(60)]
        public string EMail { get; set; }
        public DateTime RegisterDate { get; set; }=DateTime.Now;
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public bool IsDelete { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}