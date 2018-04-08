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
            if (LoggedAsAdmin())
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if (LoggedAsAdmin())
            {
                return RedirectToAction("Index");
            }
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
            if (LoggedAsAdmin())
            {
                Session["Account"] = null;
                Session["Role"] = null;

                return RedirectToAction("Login");
            }
            return RedirectToAction("Index");
        }

        public bool LoggedAsAdmin()
        {
            if (Session != null)
            {
                if (Session["Account"] != null && Session["Role"] != null)
                {
                    if (((string)Session["Role"]).Equals("Admin"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}