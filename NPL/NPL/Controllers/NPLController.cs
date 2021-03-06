﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class NPLController : Controller
    {
        // GET: NPL
        DBNPLDataContext data = new DBNPLDataContext();
        private List<MonAn> LayMonAnMoi(int count)
        {
            data.MonAns.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
            return null;
        }
        public ActionResult Index()
        {
            var monanmoi = LayMonAnMoi(8);
            return View(monanmoi);
        }
    }
}