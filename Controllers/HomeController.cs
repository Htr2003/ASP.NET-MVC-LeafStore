using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LeafStore.Models;
using System.Net;

namespace LeafStore.Controllers
{
    public class HomeController : Controller
    {
        private  DBLeafStoreEntities db = new DBLeafStoreEntities();
        public ActionResult Index()
        {
            var products = db.Products;
            Session["Adress"] = "Index";
            return View(products.ToList());
        }
        public ActionResult Details(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                    return HttpNotFound();
            }
            return View(product);
                
        }
        public ActionResult AllProducts(string searchString)
        {
            Session["Adress"] = null;
            if (searchString != null)
            {
                return View(db.Products.Where(s => s.NamePro.Contains(searchString)).ToList());
            }
            return View(db.Products.ToList());

        }
        public ActionResult GetProductsByCategory()
        {
            var categories = db.Categories.ToList();
            return PartialView("CategoriesPartialView", categories);
        }
        public ActionResult GetProductsByCateId(int id)
        {
            var products = db.Products.Where(p => p.Category1.Id ==id).ToList();
            Session["Adress"] = null;
            return View("AllProducts", products);
        }
        
        public ActionResult GetProductsInCate(string IDCate)
        {
            var products = db.Products.Where(p => p.Category == IDCate).ToList();
            return View("Index", products);
        }
    }
}