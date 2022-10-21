using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models.PagesModel
{
    public class EmployeeHotelDepartment
    {
        public Department SingleDepartment { get; set; }
        public Employee SingleEmployee { get; set; }
        public List<Hotel> HotelList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<Employee> EmployeeList { get; set; }
    }
}