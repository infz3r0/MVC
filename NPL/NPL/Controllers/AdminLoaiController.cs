﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class AdminLoaiController : Controller
    {
        private DBNPLDataContext data = new DBNPLDataContext();

        // GET: AdminLoai
        public ActionResult Index()
        {
            if (!Manager.LoggedAsAdmin())
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Loai> all = data.Loais.ToList();
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
            string tenLoai = form["TenLoai"];
            int idNhom = Convert.ToInt32(form["IDNhom"]);

            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                ViewBag.MessageFail = "Tên loại không hợp lệ";
                return View();
            }

            Loai loai = new Loai();
            loai.TenLoai = tenLoai;
            loai.IDNhom = idNhom;
            loai.SoLuong = 0;
            data.Loais.InsertOnSubmit(loai);
            data.SubmitChanges();
            ViewBag.MessageSuccess = "Thêm loại: [" + tenLoai + "] thành công";
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
            Loai loai = data.Loais.SingleOrDefault(i => i.IDLoai == id);
            return View(loai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            string tenLoai = form["TenLoai"];
            int id = Convert.ToInt32(form["IDLoai"]);
            int idNhom = Convert.ToInt32(form["IDNhom"]);
            Loai loai = data.Loais.SingleOrDefault(i => i.IDLoai == id);

            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                ViewBag.MessageFail = "Tên nhóm không hợp lệ";
                return View(loai);
            }
            string tenCu = loai.TenLoai;
            loai.TenLoai = tenLoai;
            loai.IDNhom = idNhom;
            UpdateModel(loai);
            data.SubmitChanges();
            ViewBag.MessageSuccess = "Đã thay đổi tên loại: [" + tenCu + "] => [" + tenLoai + "] thành công";
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
            Loai loai = data.Loais.SingleOrDefault(i => i.IDLoai == id);
            return View(loai);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            int id = Convert.ToInt32(form["id"]);
            Loai loai = data.Loais.SingleOrDefault(i => i.IDLoai == id);
            //Nếu nhóm tồn tại
            if (loai != null)
            {
                //Xóa nhóm
                data.Loais.DeleteOnSubmit(loai);
                try
                {
                    data.SubmitChanges();
                }
                catch (Exception ex)
                {
                    int countLoai = data.Loais.Count(i => i.IDLoai == id);
                    ViewBag.IsError = true;
                    if (ex.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    {
                        ViewBag.ErrorBody = string.Format("Không thể xóa loại [{0}] do có {1} món ăn trong loại này.", loai.TenLoai, countLoai);
                    }
                    else
                    {
                        ViewBag.ErrorBody = ex.ToString();
                    }
                    List<Loai> all = data.Loais.ToList();
                    return View("Index", all);
                }
            }
            //Xóa thành công hoặc nhóm k tồn tại thì trở về Index
            return RedirectToAction("Index", "AdminLoai");
        }

        [ChildActionOnly]
        public ActionResult PV_Dropdown_Nhom(int? idNhom)
        {
            List<Nhom> all = data.Nhoms.ToList();
            if (idNhom != null)
            {
                ViewBag.idNhom = idNhom;
            }
            return PartialView(all);
        }
        //end class
    }
}