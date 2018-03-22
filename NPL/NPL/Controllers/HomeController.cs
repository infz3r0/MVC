using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class HomeController : Controller
    {
        private DBNPLDataContext data = new DBNPLDataContext();

        public ActionResult Index()
        {
            Admin r = data.Admins.SingleOrDefault(i => i.Username == "admin");
            return View(r);
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
    }
}