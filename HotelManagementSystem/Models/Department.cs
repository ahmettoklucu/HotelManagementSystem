using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public List<Employee> Employee { get; set; }
    }
}