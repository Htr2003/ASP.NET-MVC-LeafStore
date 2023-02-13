using LeafStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LeafStore.Controllers
{
    public class CategoriesController : Controller
    {
        DBLeafStoreEntities database = new DBLeafStoreEntities();
        // GET: Categories
        public ActionResult Index()
        {
            if (Session["AdminName"]!=null)
            {
                var categories = database.Categories.ToList();
                return View(categories);
            }    
            return RedirectToAction("Login","Users");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                database.Categories.Add(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi tạo mới thể loại! ");
            }
        }
        public ActionResult Details (int id)
        {
            var category= database.Categories.Where(c => c.Id == id).FirstOrDefault();
            return View(category);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = database.Categories.Where(c => c.Id == id).FirstOrDefault();
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit (int id, Category category)
        {
            database.Entry(category).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int ? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = database.Categories.Where(c => c.Id == id).FirstOrDefault();
            if(category==null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category =database.Categories.Where(c => c.Id == id).FirstOrDefault();
                database.Categories.Remove(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Không thể xóa vì liên quan đến bảng khác");
            }
        }

    }
}