using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class AdminController : Controller
    {
        private DBNPLDataContext data = new DBNPLDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form["username"];
            string password = form["password"];

            Admin r = data.Admins.SingleOrDefault(i => i.Username == username && i.Password == password);

            if (r == null)
            {
                ViewBag.Message = "Login failed";
                return View();
            }

            Session["Account"] = r;
            Session["Role"] = "Admin";

            r.LastLogin = DateTime.Now;
            UpdateModel(r);
            data.SubmitChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            if (Session["Account"] != null)
            {
                Session["Account"] = null;
                Session["Role"] = null;
            }
            return RedirectToAction("Login");
        }

    }
}