using HotelManagementSystem.Models.PagesModel;
using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Web.UI.HtmlControls;

namespace HotelManagementSystem.Controllers
{
    public class HotelAdminController : Controller
    {
        DataContext db = new DataContext();
        // GET: HotelAdmin
        public int HotelControl()
        {
            var user = db.User.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var hotel = db.Hotel.FirstOrDefault(x => x.UserId == user.Id).Id;
            return hotel;
        }

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Room()
        {
            var hotelcontrol = HotelControl();
            RoomHotelRoomType model = new RoomHotelRoomType()
            {
                RoomList = db.Rooms.Where(x => x.HotelId == hotelcontrol && x.IsDelete == false).ToList(),
                RoomTypeList = db.RoomType.ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult RoomCreate()
        {
            RoomHotelRoomType model = new RoomHotelRoomType()
            {
                RoomTypeList = db.RoomType.Where(x => x.IsDelete == false).ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult RoomCreate(Room room)
        {

            var hotelcontrol = HotelControl();
            room.HotelId = hotelcontrol;
            db.Rooms.Add(room);
            db.SaveChanges();
            ViewBag.mesaj = "Kayıt başarili";
            RoomHotelRoomType model = new RoomHotelRoomType()
            {
                RoomTypeList = db.RoomType.Where(x => x.IsDelete == false).ToList(),
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult RoomEdit(int id)
        {
            var room = db.Rooms.Find(id);
            RoomHotelRoomType model = new RoomHotelRoomType();
            if (room != null)
            {
                model.SingleRoom = room;
                model.RoomTypeList = db.RoomType.ToList();
                return View(model);
            }
            return RedirectToAction("Room");
        }
        [HttpPost]
        public ActionResult RoomEdit(int id, Room room)
        {
            var hotelcontrol = HotelControl();
            RoomHotelRoomType model = new RoomHotelRoomType();
            var editRoom = db.Rooms.Find(id);
            if (editRoom != null)
            {
                editRoom.RoomName = room.RoomName;
                editRoom.RoomTypeId = room.RoomTypeId;
                editRoom.HotelId = hotelcontrol;
                editRoom.PeerDeim = room.PeerDeim;
                editRoom.Status = room.Status;
                db.SaveChanges();

                model.SingleRoom = editRoom;
                model.RoomTypeList = db.RoomType.ToList();

            }
            return View(model);
        }
        public ActionResult RoomDelete(int id)
        {
            var room = db.Rooms.Find(id);

            if (room != null)
            {

                room.IsDelete = true;
                db.SaveChanges();
            }
            return RedirectToAction("Room");
        }
        public ActionResult Employee()
        {
            var hotelControl = HotelControl();
            EmployeeHotelDepartment model = new EmployeeHotelDepartment()
            {
                EmployeeList = db.Employee.Where(x => x.HotelId == hotelControl).ToList(),
                DepartmentList = db.Department.ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult EmployeeCreate()
        {
            EmployeeHotelDepartment model = new EmployeeHotelDepartment()
            {
                DepartmentList = db.Department.ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EmployeeCreate(Employee employee)
        {
            var hotelcontrol = HotelControl();
            Employee addEmploye = new Employee();
            addEmploye.Name = employee.Name;
            addEmploye.Surename = employee.Surename;
            addEmploye.DepartmentId = employee.DepartmentId;
            addEmploye.HotelId = hotelcontrol;
            db.Employee.Add(addEmploye);
            db.SaveChanges();
            ViewBag.massage = "kayit başari oluşturuldu";
            EmployeeHotelDepartment model = new EmployeeHotelDepartment()
            {
                DepartmentList = db.Department.ToList(),
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult EmployeeEdit(int id)
        {
            var employee = db.Employee.Find(id);
            EmployeeHotelDepartment model = new EmployeeHotelDepartment();
            if (employee != null)
            {
                model.SingleEmployee = employee;
                model.DepartmentList = db.Department.ToList();
                return View(model);
            }
            return RedirectToAction("Employee");

        }
        [HttpPost]
        public ActionResult EmployeeEdit(int id, Employee employee)
        {
            var hotelcontrol = HotelControl();
            var editEmployee = db.Employee.Find(id);
            editEmployee.Name = employee.Name;
            editEmployee.Surename = employee.Surename;
            editEmployee.DepartmentId = employee.DepartmentId;
            editEmployee.HotelId = hotelcontrol;
            ViewBag.massage = "kayit başari ile güncellendi";
            db.SaveChanges();
            EmployeeHotelDepartment model = new EmployeeHotelDepartment()
            {
                DepartmentList = db.Department.ToList(),
            };
            return View(model);
        }

        public ActionResult Reservation()
        {
            var hotelControl = HotelControl();
            ReservationRoomHotel model = new ReservationRoomHotel()
            {
                UserList = db.User.ToList(),
                ReservationList = db.Reservation.Where(x => x.Room.HotelId == hotelControl && x.IsDelete == false).ToList(),
                RoomList = db.Rooms.Where(x => x.HotelId == hotelControl && x.IsDelete == false).ToList(),

            };
            return View(model);
        }
        [HttpGet]
        public ActionResult ReservationCreate()
        {
            var hotelcontrol = HotelControl();
            ReservationRoomHotel model = new ReservationRoomHotel()
            {
                UserList=db.User.Where(x=>x.IsDelete==false).ToList(),
                RoomList = db.Rooms.Where(x => x.HotelId == hotelcontrol && x.IsDelete == false).ToList(),
                RoomNotAvailableList = db.RoomNotAvailables.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ReservationCreate(Reservation reservation)
        {
            var hotelcontrol = HotelControl();


            var roomnotavailable = db.RoomNotAvailables.Where(x => x.Date >= reservation.StartDate && x.Date <= reservation.EndDate && x.RoomId == reservation.RoomId).ToList().Count();
            if (roomnotavailable == 0)
            {
                Reservation addReservation = new Reservation();
                addReservation.StartDate = reservation.StartDate;
                addReservation.EndDate = reservation.EndDate;
                addReservation.RoomId = reservation.RoomId;
                addReservation.UserId = reservation.UserId;
                DateTime start = new DateTime(addReservation.StartDate.Year, addReservation.StartDate.Month, addReservation.StartDate.Day);
                DateTime end = new DateTime(addReservation.EndDate.Year, addReservation.EndDate.Month, addReservation.EndDate.Day);
                TimeSpan time = end - start;
                double day = time.TotalDays;
                addReservation.IsDelete = false;
                db.Reservation.Add(addReservation);
                db.SaveChanges();
                for (int i = 0; i <= day; i++)
                {
                    RoomNotAvailable roomNotAvailable1 = new RoomNotAvailable();
                    roomNotAvailable1.RoomId = addReservation.RoomId;
                    roomNotAvailable1.Date = start.AddDays(i);
                    roomNotAvailable1.ReservationId = addReservation.Id;
                    db.RoomNotAvailables.Add(roomNotAvailable1);
                    db.SaveChanges();
                }

                db.SaveChanges();
                ViewBag.massage = "kayit başaari oluşturuldu";
               
            }


            ReservationRoomHotel model = new ReservationRoomHotel()
            {
                
                UserList = db.User.Where(x => x.IsDelete == false).ToList(),
                RoomList = db.Rooms.Where(x => x.HotelId == hotelcontrol && x.IsDelete == false).ToList(),
                RoomNotAvailableList = db.RoomNotAvailables.ToList()
            };
            return View(model);
        }
        public ActionResult ReservationEdit(int id)
        {
            var reservation = db.Reservation.Find(id);
            if (reservation != null)
            {
                var hotelcontrol = HotelControl();
                ReservationRoomHotel model = new ReservationRoomHotel()
                {
                    UserList = db.User.Where(x => x.IsDelete == false).ToList(),
                    RoomList = db.Rooms.Where(x => x.HotelId == hotelcontrol && x.IsDelete == false).ToList(),
                    RoomNotAvailableList = db.RoomNotAvailables.ToList()
                };
                return View(model);
            }
            return RedirectToAction("Reservation");

        }
        [HttpPost]
        public ActionResult ReservationEdit(int id, Reservation reservation)
        {
            var hotelcontrol = HotelControl();
            var editReservation = db.Reservation.Find(id);
            var roomnotavailable = db.RoomNotAvailables.Where(x => x.ReservationId == reservation.Id).ToList();
            foreach (var item in roomnotavailable)
            {
                db.RoomNotAvailables.Remove(item);
            }

            roomnotavailable = db.RoomNotAvailables.Where(x => x.Date >= reservation.StartDate && x.Date <= reservation.EndDate && x.RoomId == reservation.RoomId).ToList();
            if (roomnotavailable == null)
            {
                Reservation addReservation = new Reservation();
                editReservation.StartDate = reservation.StartDate;
                editReservation.EndDate = reservation.EndDate;
                editReservation.RoomId = reservation.RoomId;
                DateTime start = new DateTime(editReservation.StartDate.Year, editReservation.StartDate.Month, editReservation.StartDate.Day);
                DateTime end = new DateTime(editReservation.EndDate.Year, editReservation.EndDate.Month, editReservation.EndDate.Day);
                TimeSpan time = end - start;
                double day = time.TotalDays;

                for (int i = 0; i <= day; i++)
                {
                    RoomNotAvailable roomNotAvailable1 = new RoomNotAvailable();
                    roomNotAvailable1.RoomId = addReservation.RoomId;
                    roomNotAvailable1.Date = start.AddDays(i);
                    roomNotAvailable1.ReservationId = editReservation.Id;
                    db.RoomNotAvailables.Add(roomNotAvailable1);
                }

                ReservationRoomHotel model = new ReservationRoomHotel()
                {
                    RoomList = db.Rooms.Where(x => x.HotelId == hotelcontrol && x.IsDelete == false && x.Status == true).ToList(),
                };


                ViewBag.massage = "kayit başari ile güncellendi";
                db.SaveChanges();
                return View();
            }
            else
            {
                ViewBag.masage = "oda bu tarihler arasinda doludur lütfen odayi veya tarihi değiştiriniz.";
                return View();
            }

        }
        public ActionResult ReservationDelete(int id, Reservation reservation)
        {
            var hotelcontrol = HotelControl();
            var deleteReservation = db.Reservation.Find(id);
            var roomnotavailable = db.RoomNotAvailables.Where(x => x.ReservationId == reservation.Id).ToList();
            foreach (var item in roomnotavailable)
            {
                db.RoomNotAvailables.Remove(item);
            }
            deleteReservation.IsDelete = true;
            db.SaveChanges();
            return View();

        }
    }
}