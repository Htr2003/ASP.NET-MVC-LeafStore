using LeafStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace LeafStore.Controllers
{
    public class UsersController : Controller
    {
        private DBLeafStoreEntities database = new DBLeafStoreEntities();
        // GET: Users

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer cust)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Không được để trống họ và tên");
                if (string.IsNullOrEmpty(cust.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Không được để trống số điện thoại");
                if (string.IsNullOrEmpty(cust.EmailCus))
                    ModelState.AddModelError(string.Empty, "Không được để trống Email");
                if (string.IsNullOrEmpty(cust.AccountCus))
                    ModelState.AddModelError(string.Empty, "Không được để trống tên đăng nhập");
                if (string.IsNullOrEmpty(cust.PasswordCus))
                    ModelState.AddModelError(string.Empty, "Không được bỏ trống mật khẩu");

                //Kiểm tra tên đăng nhập có tồn tại không
                var cus = database.Customers.FirstOrDefault(k => k.AccountCus == cust.AccountCus);
                if (cus != null)
                {
                    ModelState.AddModelError(string.Empty, "Tên tài khoảng đã được dùng! Vui lòng chọn tên khác");
                }

                if (ModelState.IsValid)
                {
                    database.Customers.Add(cust);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer acc)
        {
            
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(acc.AccountCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(acc.PasswordCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                    
                var checkAdmin = database.AdminUsers.FirstOrDefault(s => s.NameUser == acc.AccountCus && s.PasswordUser == acc.PasswordCus);
                if (checkAdmin != null)
                {
                    Session["AdminName"] = checkAdmin.NameUser;
                    return RedirectToAction("Index", "Admin");
                }
                
                var check = database.Customers.Where(s => s.AccountCus.Equals(acc.AccountCus) && s.PasswordCus.Equals(acc.PasswordCus)).FirstOrDefault();
                if (check != null)
                {
                    Session["IDCus"] = check.IDCus;
                    Session["NameCus"] = check.NameCus;
                    Session["PassCus"] = check.PasswordCus;
                    Session["TaiKhoan"] = check;
                    return RedirectToAction("Index", "Home");

                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";

            }
            return View();

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult Detail(int idCus=-1)
        {
            if (idCus != -1)
            {
                var info = database.Customers.FirstOrDefault(s => s.IDCus == idCus);
                return View(info);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult Edit(int ? IDCus)
        {
            var cus = database.Customers.Where(s => s.IDCus == IDCus).FirstOrDefault();
            return View(cus);
        }
        [HttpPost]
        public ActionResult Edit(int id, Customer cus)
        {
            database.Entry(cus).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("ListAccount");
        }
        public ActionResult ListAccount()
        {
            if (Session["AdminName"]!=null)
            {
                var kh = database.Customers.ToList();
                return View(kh);
            }
            return RedirectToAction("Login", "Users");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var customer = database.Customers.Where(c => c.IDCus == id).FirstOrDefault();
                database.Customers.Remove(customer);
                database.SaveChanges();
                return RedirectToAction("ListAccount");
            }
            catch
            {
                return Content("Không thể xóa vì liên quan đến bảng khác");
            }
        }
    }
}
