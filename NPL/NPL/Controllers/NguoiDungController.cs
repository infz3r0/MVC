using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPL.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }
    }
}