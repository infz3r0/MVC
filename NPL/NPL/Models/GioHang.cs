using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPL.Models
{

    public class GioHang
    {
        DBNPLDataContext data = new DBNPLDataContext();
        public int iIDMonAn { set; get; }
        public string sTenMonAn { set; get; }
        public string sHinhAnh { set; get; }
        public double dPhiVanChuyen { set; get; }
        public int iIDThucDon { set; get; }
        public string sTenThucDon { set; get; }
        public double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public double dThanhTien 
        {
            get { return (dDonGia * iSoLuong) + dPhiVanChuyen; }
        }
        //khoi tao gio hang theo ma thuc don duoc truyen vao




    }
}