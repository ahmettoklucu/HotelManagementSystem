using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class Role
    {

        [Key]
        public int Id { get; set; }
        [StringLength(15)]
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}