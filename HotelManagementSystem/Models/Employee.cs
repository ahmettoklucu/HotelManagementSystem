using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public Decimal Wage { get; set; }
        public bool IsDelete { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
    }
}