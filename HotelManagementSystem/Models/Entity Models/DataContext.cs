using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DataContext:DbContext
    {
        public DataContext() : base("DbConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Income_ExpenditureTable> Income_ExpenditureTables { get; set; }
        public DbSet<RoomNotAvailable> RoomNotAvailables { get; set; }

    }
}