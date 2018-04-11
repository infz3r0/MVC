using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class AdminMonAnController : Controller
    {
        private DBNPLDataContext data = new DBNPLDataContext();

        // GET: AdminMonAn
        public ActionResult Index()
        {
            if (!Manager.LoggedAsAdmin())
            {
                return RedirectToAction("Login", "Admin");
            }
            List<MonAn> all = data.MonAns.ToList();
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
        public ActionResult Create(FormCollection form, HttpPostedFileBase HinhAnh)
        {
            string tenMonAn = form["TenMonAn"];
            string gioiThieu = form["GioiThieu"];
            decimal phiVanChuyen = Convert.ToDecimal(form["PhiVanChuyen"]);
            int idLoai = Convert.ToInt32(form["IDLoai"]);

            ViewBag.MessageFail = string.Empty;
            if (string.IsNullOrWhiteSpace(tenMonAn))
            {
                ViewBag.MessageFail += "Tên món ăn không hợp lệ. ";
            }
            if (Convert.ToInt32(phiVanChuyen) % 500 != 0)
            {
                ViewBag.MessageFail += "Phí vận chuyển không hợp lệ. ";
            }
            if (!string.IsNullOrEmpty(ViewBag.MessageFail))
            {
                return View();
            }

            string _FileName = "";
            try
            {
                if (HinhAnh.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(HinhAnh.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Images/MonAn/"), _FileName);
                    if (!System.IO.File.Exists(_path))
                    {
                        HinhAnh.SaveAs(_path);
                    }                    
                }
            }
            catch
            {                
            }

            MonAn monAn = new MonAn();
            monAn.TenMonAn = tenMonAn;
            monAn.GioiThieu = gioiThieu;
            monAn.HinhAnh = _FileName;
            monAn.PhiVanChuyen = phiVanChuyen;
            monAn.IDLoai = idLoai;

            data.MonAns.InsertOnSubmit(monAn);
            data.SubmitChanges();
            ViewBag.MessageSuccess = "Thêm món ăn: [" + tenMonAn + "] thành công";
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
            MonAn monAn = data.MonAns.SingleOrDefault(i => i.IDMonAn == id);
            return View(monAn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            string tenMonAn = form["TenMonAn"];
            int id = Convert.ToInt32(form["IDMonAn"]);
            MonAn monAn = data.MonAns.SingleOrDefault(i => i.IDMonAn == id);

            if (string.IsNullOrWhiteSpace(tenMonAn))
            {
                ViewBag.MessageFail = "Tên món ăn không hợp lệ";
                return View(monAn);
            }
            string tenCu = monAn.TenMonAn;
            monAn.TenMonAn = tenMonAn;
            UpdateModel(monAn);
            data.SubmitChanges();
            ViewBag.MessageSuccess = "Đã thay đổi tên món ăn: [" + tenCu + "] => [" + tenMonAn + "] thành công";
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
            MonAn monAn = data.MonAns.SingleOrDefault(i => i.IDMonAn == id);
            return View(monAn);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            int id = Convert.ToInt32(form["id"]);
            MonAn monAn = data.MonAns.SingleOrDefault(i => i.IDMonAn == id);
            //Nếu nhóm tồn tại
            if (monAn != null)
            {
                //Xóa nhóm
                data.MonAns.DeleteOnSubmit(monAn);
                try
                {
                    data.SubmitChanges();
                }
                catch (Exception ex)
                {
                    int countThucDon = data.ThucDons.Count(i => i.IDMonAn == id);
                    ViewBag.IsError = true;
                    if (ex.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    {
                        ViewBag.ErrorBody = string.Format("Không thể xóa món ăn [{0}] do có {1} thực đơn trong món ăn này.", monAn.TenMonAn, countThucDon);
                    }
                    else
                    {
                        ViewBag.ErrorBody = ex.ToString();
                    }
                    List<MonAn> all = data.MonAns.ToList();
                    return View("Index", all);
                }
            }
            //Xóa thành công hoặc nhóm k tồn tại thì trở về Index
            return RedirectToAction("Index", "AdminMonAn");
        }

        [ChildActionOnly]
        public ActionResult PV_Dropdown_Loai(int? idLoai)
        {
            List<Loai> all = data.Loais.OrderBy(i=>i.TenLoai).ToList();
            if (idLoai != null)
            {
                ViewBag.idLoai = idLoai;
            }
            return PartialView(all);
        }
        //end class
    }
}