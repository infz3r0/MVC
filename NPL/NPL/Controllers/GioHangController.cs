﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPL.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace NPL.Controllers
{
    public class GioHangController : Controller
    {
        DBNPLDataContext data = new DBNPLDataContext();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int iIDThucDon, string strURL)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.Find(n => n.iIDThucDon == iIDThucDon);
            if (sanpham == null)
            {
                sanpham = new GioHang(iIDThucDon);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        public double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {
            //----------------
            List<GioHang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(int iMaSP)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iIDThucDon == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iIDThucDon == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iIDThucDon == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatcaGiohang()
        {
            List<GioHang> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]

        public ActionResult DatHang()
        {
            if (Session["Username"] == null || Session["Username"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGiohang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);

        }

        public ActionResult DatHang(FormCollection collection)
        {
            DatHang ddh = new DatHang();            
            TaiKhoan tk = (TaiKhoan)Session["Username"];
            List<GioHang> gh = LayGioHang();
            ddh.IDUser = tk.IDUser;
            ddh.ThoiGianDatHang = DateTime.Now;            
            ddh.DaGiaoHang = false;
            var diachigiaohang = collection["DiaChiGiaoHang"];            
                ddh.DiaChiGiaoHang = diachigiaohang;
            double iTongTien = 0;           
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            ddh.ThanhTien = decimal.Parse( iTongTien.ToString());
            data.DatHangs.InsertOnSubmit(ddh);
                data.SubmitChanges();            
            foreach (var item in gh)
                {
                    ChiTietDatHang ctdn = new ChiTietDatHang();
                    ctdn.ID = ddh.ID;
                    
                    ctdn.IDThucDon = item.iIDThucDon;
                    ctdn.SoLuong = item.iSoLuong;  
                    data.ChiTietDatHangs.InsertOnSubmit(ctdn);
                }            
                data.SubmitChanges();
                Session["GioHang"] = null;
                return RedirectToAction("Xacnhandonhang", "GioHang");           
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}
    
