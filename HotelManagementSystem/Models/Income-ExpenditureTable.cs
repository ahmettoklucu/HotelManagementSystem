using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class Income_ExpenditureTable
    {
        [Key]
        public int Id { get; set; }
        public int Description { get; set; }
        public decimal İncomeAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel hotel { get; set; }
        public bool IsDelete { get; set; }
    }
}