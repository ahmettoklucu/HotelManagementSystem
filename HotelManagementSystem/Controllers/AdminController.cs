using HotelManagementSystem.Models;
using HotelManagementSystem.Models.PagesModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace HotelManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        DataContext db = new DataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User user)
        {
            var userControl = db.User.FirstOrDefault(x => x.UserName == user.UserName);

            if (userControl != null)
            {
                ViewBag.Mesaj = "Kullanıcı adı sisteme kayıtlı. Lütfen farklı bir hesap ismi ile devam ediniz yada giriş yapınız.";
            }
            else
            {
                Regex _password = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#$%@!?.,_^*+/-]).{8,}$");

                if (_password.IsMatch(user.Password) == false)
                {
                    ViewBag.Mesaj = "Şifre tanımlarken  en az 8 karakter ve 1 büyük harf, 1 küçük harf, rakam ve özel karakter içermelidir.";
                }
                else
                {
                    user.RoleId = 1;
                    db.User.Add(user);
                    db.SaveChanges();
                    ViewBag.Mesaj = "Tebrikler kaydınız başarlı rezervasyon yapabilirsiniz.";
                }
            }
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string Username, string Password, bool? Remember)
        {
            var userControl = db.User.FirstOrDefault(x => x.UserName == Username && x.Password == Password);
            if (userControl != null)
            {
                if (userControl.IsDelete == false)
                {
                    bool benihatirla = Remember == null ? false : true;
                    FormsAuthentication.SetAuthCookie(userControl.UserName, benihatirla);
                    ViewBag.Mesaj = "Merhaba " + Username;
                    if (userControl.RoleId == 3)
                    {
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("index");
                    }

                }
                else
                {
                    ViewBag.Mesaj = "Bu kullanıcı hesabı silinmiştir";
                }

            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı Adı veya Şifre Hatalı";
            }


            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Hotel()
        {
            RoomHotelRoomType model = new RoomHotelRoomType();
            model.HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList();
            model.RoomList = db.Rooms.Where(x => x.IsDelete == false).ToList();
            model.UserList = db.User.Where(x => x.IsDelete == false).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult HotelCreate()
        {
            HotelUserModel model = new HotelUserModel() { UserList = db.User.Where(x => x.IsDelete == false).ToList() };
            return View(model);
        }
        [HttpPost]
        public ActionResult HotelCreate(Hotel hotel, HttpPostedFileBase Image)
        {
            Hotel addHotel = new Hotel();
            if (Image != null && Image.ContentLength > 0)
            {
                string ImagePath = "";
                string ImageName = "";

                ImageName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(Image.FileName);
                ImagePath = Path.Combine(Server.MapPath("~/Content/images/Hotel"), ImageName);
                Image.SaveAs(ImagePath);
                addHotel.Image = ImageName;
            }
            else
            {
                addHotel.Image = null;
            }
            addHotel.Name = hotel.Name;
            addHotel.Adress = hotel.Adress;
            addHotel.CityName = hotel.CityName;
            addHotel.UserId = hotel.UserId;
            addHotel.IsDelete = false;
            db.Hotel.Add(addHotel);
            var editUser = db.User.Find(addHotel.UserId);
            editUser.RoleId = 2;
            db.SaveChanges();
            HotelUserModel model = new HotelUserModel() { UserList = db.User.Where(x => x.IsDelete == false).ToList() };
            return View(model);
        }
        [HttpGet]
        public ActionResult HotelEdit(int id)
        {
            var hotel = db.Hotel.Find(id);
            var editUser = db.User.Find(hotel.UserId);
            editUser.RoleId = 3;
            HotelUserModel model = new HotelUserModel();
            if (hotel != null)
            {
                model.SingleHotel = hotel;
                model.UserList = db.User.Where(x => x.IsDelete == false).ToList();
                return View(model);
            }
            return RedirectToAction("Hotel");
        }
        [HttpPost]

        public ActionResult HotelEdit(Hotel hotel, HttpPostedFileBase Image)
        {
            var editHotel = db.Hotel.Find(hotel.Id);
            HotelUserModel model = new HotelUserModel();
            if (editHotel != null)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    string ImagePath = "";
                    string ImageName = "";
                    ImageName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(Image.FileName);
                    ImagePath = Path.Combine(Server.MapPath("~/Content/images/Hotel"), ImageName);
                    Image.SaveAs(ImagePath);
                    editHotel.Image = ImageName;
                }
                editHotel.Name = hotel.Name;
                editHotel.CityName = hotel.CityName;
                editHotel.Name = hotel.Name;
                editHotel.UserId = hotel.UserId;
                var editUser = db.User.Find(hotel.UserId);
                editUser.RoleId = 2;

                ViewBag.massage = "kayit başari ile güncellendi";
                db.SaveChanges();

                model.SingleHotel = editHotel;
                model.UserList = db.User.Where(x => x.IsDelete == false).ToList();
            }
            else
            {

            }
            return View(model);
        }

        public ActionResult HotelDelete(int id)
        {
            var hotel = db.Hotel.Find(id);
            var roomList = db.Rooms.Where(x => x.HotelId == id);
            var employeeList = db.Employee.Where(x => x.HotelId == id);
            if (hotel != null)
            {
                hotel.IsDelete = true;
                foreach (var item in roomList)
                {
                    item.IsDelete = true;
                }
                foreach (var item in employeeList)
                {
                    item.IsDelete = true;
                }
                var editUser = db.User.Find(hotel.UserId);
                editUser.RoleId = 3;
                db.SaveChanges();
            }
            return RedirectToAction("Hotel");
        }
        public ActionResult Room()
        {
            RoomHotelRoomType model = new RoomHotelRoomType();
            model.RoomList = db.Rooms.Where(x => x.IsDelete == false).ToList();
            model.HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList();
            model.RoomTypeList = db.RoomType.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult RoomCreate()
        {
            RoomHotelRoomType model = new RoomHotelRoomType()
            {
                RoomTypeList = db.RoomType.Where(x => x.IsDelete == false).ToList(),
                HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult RoomCreate(Room room)
        {
            //Room addRoom = new Room();


            db.Rooms.Add(room);
            db.SaveChanges();
            ViewBag.mesaj = "Kayıt başarili";
            RoomHotelRoomType model = new RoomHotelRoomType()
            {
                RoomTypeList = db.RoomType.Where(x => x.IsDelete == false).ToList(),
                HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList()
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
                model.HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList();
                return View(model);
            }
            return RedirectToAction("Room");
        }
        [HttpPost]
        public ActionResult RoomEdit(int id, Room room)
        {
            RoomHotelRoomType model = new RoomHotelRoomType();
            var editRoom = db.Rooms.Find(id);
            if (editRoom != null)
            {
                editRoom.RoomName = room.RoomName;
                editRoom.RoomTypeId = room.RoomTypeId;
                editRoom.HotelId = room.HotelId;
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
            EmployeeHotelDepartment model = new EmployeeHotelDepartment();
            model.EmployeeList = db.Employee.Where(x => x.IsDelete == false).ToList();
            model.DepartmentList = db.Department.Where(x => x.IsDelete == false).ToList();
            model.HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult EmployeeCreate()
        {
            EmployeeHotelDepartment model = new EmployeeHotelDepartment()
            {
                DepartmentList = db.Department.ToList(),
                HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EmployeeCreate(Employee employee)
        {
            Employee addEmploye = new Employee();
            addEmploye.Name = employee.Name;
            addEmploye.Surename = employee.Surename;
            addEmploye.DepartmentId = employee.DepartmentId;
            addEmploye.HotelId = employee.HotelId;
            db.Employee.Add(addEmploye);
            db.SaveChanges();
            ViewBag.massage = "kayit başari oluşturuldu";
            EmployeeHotelDepartment model = new EmployeeHotelDepartment()
            {
                DepartmentList = db.Department.ToList(),
                HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList()
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
                model.HotelList = db.Hotel.Where(x => x.IsDelete == false).ToList();
                return View(model);
            }
            return RedirectToAction("Employee");

        }
        [HttpPost]
        public ActionResult EmployeeEdit(int id, Employee employee)
        {
            var editEmployee = db.Employee.Find(id);
            editEmployee.Name = employee.Name;
            editEmployee.Surename = employee.Surename;
            editEmployee.DepartmentId = employee.DepartmentId;
            editEmployee.HotelId = employee.HotelId;
            ViewBag.massage = "kayit başari ile güncellendi";
            db.SaveChanges();
            return View();
        }
        public ActionResult EmployeeDelete(int id)
        {
            var employee = db.Employee.Find(id);
            if (employee != null)
            {

                employee.IsDelete = true;
                ViewBag.massage = "kayit başari ile silindi";
                db.SaveChanges();
            }

            return RedirectToAction("Employee");
        }
        public ActionResult User()
        {
            var User = db.User.Where(x => x.IsDelete == false).ToList();
            return View(User);
        }
        [HttpGet]
        public ActionResult UserCreate()
        {
            UserRoleModel model = new UserRoleModel()
            {
                RoleList = db.Roles.ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult UserCreate(User user)
        {
            var userControl = db.User.FirstOrDefault(x => x.UserName == user.UserName);

            if (userControl != null)
            {
                ViewBag.Mesaj = "Kullanıcı adı sisteme kayıtlı. Lütfen farklı bir hesap ismi ile devam ediniz yada giriş yapınız.";
            }
            else
            {
                Regex _password = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#$%@!?.,_^*+/-]).{8,}$");

                if (_password.IsMatch(user.Password) == false)
                {
                    ViewBag.Mesaj = "Şifre tanımlarken  en az 8 karakter ve 1 büyük harf, 1 küçük harf, rakam ve özel karakter içermelidir.";
                }
                else
                {
                    user.RoleId = 3;
                    db.User.Add(user);
                    db.SaveChanges();
                    ViewBag.Mesaj = "Tebrikler kaydınız başarlı bir şekilde oluşturuldu. Rezervasyon yapabilirsiniz.";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult UserEdit(int id)
        {
            var user = db.User.Find(id);
            UserRoleModel model = new UserRoleModel();
            if (user != null)
            {
                model.SingleUser = user;
                model.RoleList = db.Roles.ToList();
                return View(model);
            }
            return RedirectToAction("User");

        }
        [HttpPost]
        public ActionResult UserEdit(int id, User user)
        {
            var editUser = db.User.Find(id);
            var userControl = db.User.FirstOrDefault(x => x.UserName == user.UserName);

            if (userControl != null)
            {
                ViewBag.Mesaj = "Kullanıcı adı sisteme kayıtlı. Lütfen farklı bir hesap ismi ile devam ediniz.";
            }
            else
            {
                Regex _password = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#$%@!?.,_^*+/-]).{8,}$");

                if (_password.IsMatch(userControl.Password) == false)
                {
                    ViewBag.Mesaj = "Şifre tanımlarken  en az 8 karakter ve 1 büyük harf, 1 küçük harf, rakam ve özel karakter içermelidir.";
                }
                else
                {
                    editUser.Name = user.Name;
                    editUser.Surename = user.Surename;
                    editUser.UserName = user.UserName;
                    editUser.CityName = user.CityName;
                    editUser.Password = user.Password;
                    editUser.PhoneNumber = user.PhoneNumber;
                    db.SaveChanges();
                    ViewBag.Mesaj = "Tebrikler bilgileriniz güncellenmiştir";
                }
            }
            return View();
        }
        public ActionResult Department()
        {
            var departments = db.Department.Where(X => X.IsDelete == false).ToList();
            return View(departments);
        }
        [HttpGet]
        public ActionResult DepartmentCreate()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DepartmentCreate(Department department)
        {
            Department addDepartment = new Department();
            addDepartment.Name = addDepartment.Name;

            db.Department.Add(addDepartment);
            db.SaveChanges();
            ViewBag.massage = "kayit başari ile oluşturuldu";
            return View();
        }
        [HttpGet]
        public ActionResult DepartmentEdit(int id)
        {
            var department = db.Department.Find(id);
            EmployeeHotelDepartment model = new EmployeeHotelDepartment();
            if (department != null)
            {
                model.SingleDepartment = department;
                return View(department);
            }

            return RedirectToAction("Department");
        }

        [HttpPost]
        public ActionResult DepartmentEdit(int id, Department department)
        {
            if (department != null)
            {
                var editDepartment = db.Department.Find(id);
                editDepartment.Name = department.Name;
                ViewBag.massage = "kayit başari ile güncellendi";
                db.SaveChanges();

            }
            return View(department);
        }
        public ActionResult DepartmentDelete(int id)
        {
            var department = db.Department.Find(id);
            if (department != null)
            {

                department.IsDelete = true;
                ViewBag.massage = "kayit başari ile silindi";
                db.SaveChanges();
            }

            return RedirectToAction("Employee");
        }
        public ActionResult RoomType()
        {
            var roomType = db.RoomType.Where(X => X.IsDelete == false).ToList();
            return View(roomType);
        }
        [HttpGet]
        public ActionResult RoomTypeCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RoomTypeCreate(RoomType roomType)
        {
            RoomType addRoomType = new RoomType();
            addRoomType.Name = roomType.Name;

            db.RoomType.Add(addRoomType);
            db.SaveChanges();
            ViewBag.massage = "kayit başari oluşturuldu";
            return View();
        }
        [HttpGet]
        public ActionResult RoomTypeEdit(int id)
        {
            var roomType = db.RoomType.Find(id);
            if (roomType != null)
            {
                return View(roomType);
            }

            return RedirectToAction("RoomType");
        }

        [HttpPost]
        public ActionResult RoomTypeEdit(int id, RoomType roomType)
        {
            var editRoomType = db.RoomType.Find(id);
            if (roomType != null)
            {
                editRoomType.Name = roomType.Name;
                ViewBag.massage = "kayit başari ile güncellendi";
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult RoomTypeDelete(int id)
        {
            var roomType = db.RoomType.Find(id);
            if (roomType != null)
            {

                roomType.IsDelete = true;
                ViewBag.massage = "kayit başari ile silindi";
                db.SaveChanges();
            }

            return RedirectToAction("RoomType");
        }
    }
}