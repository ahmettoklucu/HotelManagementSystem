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
    public class HomeController : Controller
    {
        public int UserControl()
        {
            var user = db.User.FirstOrDefault(x => x.UserName == User.Identity.Name).Id;
            return user;
        }
        DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Reservartion()
        {
            var userControl = UserControl();
            ReservationRoomHotel model = new ReservationRoomHotel()
            {
                ReservationList = db.Reservation.Where(x => x.Room.HotelId == userControl && x.IsDelete == false).ToList(),
                RoomList = db.Rooms.Where(x =>x.IsDelete == false).ToList(),

            };
            return View(model);
        }
        [HttpGet]
        public ActionResult ReservationCreate()
        {
            var userControl = UserControl();
            ReservationRoomHotel model = new ReservationRoomHotel()
            {
                HotelList=db.Hotel.Where(x => x.IsDelete == false).ToList(),
                RoomList = db.Rooms.Where(x=>x.IsDelete == false).ToList(),
                RoomNotAvailableList = db.RoomNotAvailables.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ReservationCreate(Reservation reservation)
        {
            var userControl = UserControl();



            var roomnotavailable = db.RoomNotAvailables.Where(x => x.Date >= reservation.StartDate && x.Date <= reservation.EndDate && x.RoomId == reservation.RoomId).ToList().Count();
            if (roomnotavailable == 0)
            {
                Reservation addReservation = new Reservation();
                addReservation.StartDate = reservation.StartDate;
                addReservation.EndDate = reservation.EndDate;
                addReservation.RoomId = reservation.RoomId;
                addReservation.UserId = userControl;
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

                HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList(),
                RoomList = db.Rooms.Where(x => x.IsDelete == false && x.Status == true).ToList(),
                RoomNotAvailableList = db.RoomNotAvailables.ToList()
            };
            return View(model);
        }
        public ActionResult ReservationEdit(int id)
        {
            var reservation = db.Reservation.Find(id);
            if (reservation != null)
            {
                ReservationRoomHotel model = new ReservationRoomHotel()
                {
                    HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList(),
                    RoomList = db.Rooms.Where(x => x.IsDelete == false && x.Status == true).ToList(),
                    RoomNotAvailableList = db.RoomNotAvailables.ToList()
                };
                return View(model);
            }
            return RedirectToAction("Reservation");

        }
        [HttpPost]
        public ActionResult ReservationEdit(int id, Reservation reservation)
        {
            var userControl = UserControl();
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
                editReservation.UserId = userControl;
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
                    RoomList = db.Rooms.Where(x =>x.IsDelete == false && x.Status == true).ToList(),
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
        public ActionResult Hotel()
        {
            var hotel = db.Hotel.Where(x =>x.IsDelete == false).ToList();
            return View(hotel);
        }
        public ActionResult HotelDetail(int id)
        {
            var hotel = db.Hotel.Find(id);

            if (hotel != null)
            {
                return View(hotel);
            }
            else
            {
                return RedirectToAction("index");
            }
        }
    }
}