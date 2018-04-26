﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;

namespace NPL.Controllers
{
    public class NguoiDungController : Controller
    {
        DBNPLDataContext data = new DBNPLDataContext();
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

        [HttpPost]

        public ActionResult DangKy(FormCollection collection, TaiKhoan tk)
        {
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var hoten = collection["HoTen"];
            bool gioitinh =Convert.ToBoolean(Convert.ToInt16( collection["GioiTinh"]));
            var dienthoai = collection["DienThoai"];
            var email = collection["Email"];     
            
            if (nhaplaimatkhau != matkhau)
            {
                ViewData["Loi1"] = "Mật khẩu không trùng khớp";
            }            
            else
            {
                tk.Username = tendn;
                tk.Password = matkhau;
                tk.HoTen = hoten;
                tk.GioiTinh = gioitinh;
                tk.Sdt = dienthoai;
                tk.Email = email;
                data.TaiKhoans.InsertOnSubmit(tk);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]

        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];

            TaiKhoan tk = data.TaiKhoans.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
            if (tk != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["Username"] = tk;
                return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.Thongbao = "Sai thông tin đăng nhập!";
            return View();
        }
        public ActionResult HienTenDN(int id)
        {
            TaiKhoan tk = (TaiKhoan)Session["Username"];
            return PartialView(tk);
        }

        public ActionResult TenDN()
        {
            return View();
        }
        
        public ActionResult TaiKhoan()
        {
            TaiKhoan tk = (TaiKhoan)Session["Username"];
            return PartialView(tk);
        }
    }
}