﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class AdminNhomController : Controller
    {
        private DBNPLDataContext data = new DBNPLDataContext();

        // GET: AdminNhom
        public ActionResult Index()
        {
            if (!Manager.LoggedAsAdmin())
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Nhom> all = data.Nhoms.ToList();
            return View(all);
        }

        public ActionResult Create()
        {
            if (!Manager.LoggedAsAdmin())
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            string tenNhom = form["TenNhom"];
            if (string.IsNullOrWhiteSpace(tenNhom))
            {
                ViewBag.MessageFail = "Tên nhóm không hợp lệ";
                return View();
            }
            Nhom nhom = new Nhom();
            nhom.TenNhom = tenNhom;
            nhom.SoLuong = 0;
            data.Nhoms.InsertOnSubmit(nhom);
            data.SubmitChanges();
            ViewBag.MessageSuccess = "Thêm nhóm: [" + tenNhom + "] thành công";
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (!Manager.LoggedAsAdmin())
            {
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Nhom nhom = data.Nhoms.SingleOrDefault(i => i.IDNhom == id);
            return View(nhom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            string tenNhom = form["TenNhom"];
            int id = Convert.ToInt32(form["IDNhom"]);
            Nhom nhom = data.Nhoms.SingleOrDefault(i => i.IDNhom == id);

            if (string.IsNullOrWhiteSpace(tenNhom))
            {
                ViewBag.MessageFail = "Tên nhóm không hợp lệ";
                return View(nhom);
            }
            string tenCu = nhom.TenNhom;
            nhom.TenNhom = tenNhom;
            UpdateModel(nhom);
            data.SubmitChanges();
            ViewBag.MessageSuccess = "Đã thay đổi tên nhóm: [" + tenCu + "] => [" + tenNhom + "] thành công";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (!Manager.LoggedAsAdmin())
            {
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Nhom nhom = data.Nhoms.SingleOrDefault(i => i.IDNhom == id);
            return View(nhom);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            int id = Convert.ToInt32(form["id"]);
            Nhom nhom = data.Nhoms.SingleOrDefault(i => i.IDNhom == id);
            //Nếu nhóm tồn tại
            if (nhom != null)
            {
                //Xóa nhóm
                data.Nhoms.DeleteOnSubmit(nhom);
                try
                {
                    data.SubmitChanges();
                }
                catch (Exception ex)
                {
                    int countLoai = data.Loais.Count(i => i.IDNhom == id);
                    ViewBag.IsError = true;
                    if (ex.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    {
                        ViewBag.ErrorBody = string.Format("Không thể xóa nhóm [{0}] do có {1} loại thức ăn trong nhóm này.", nhom.TenNhom, countLoai);
                    }
                    else
                    {
                        ViewBag.ErrorBody = ex.ToString();
                    }
                    List<Nhom> all = data.Nhoms.ToList();
                    return View("Index", all);
                }
            }
            //Xóa thành công hoặc nhóm k tồn tại thì trở về Index
            return RedirectToAction("Index", "AdminNhom");
        }

        //end class
    }
}