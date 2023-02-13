using LeafStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeafStore.Controllers
{
    public class AdminController : Controller
    {
        DBLeafStoreEntities db = new DBLeafStoreEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

    }
}