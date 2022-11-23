using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using NguyenChiTrung_ShopYAME.Models;

namespace NguyenChiTrung_ShopYAME.Controllers
{
    public class YeuThichController : Controller
    {
        DataShopYameDataContext db = new DataShopYameDataContext();
        public List<CHITIETSANPHAM> LayYeuThich()
        {
            List<CHITIETSANPHAM> lstYeuThich = Session["YeuThich"] as List<CHITIETSANPHAM>;
            if (lstYeuThich == null)
            {
                //Khởi tạo Giỏ hàng (giỏ hàng chưa tồn tại)
                lstYeuThich = new List<CHITIETSANPHAM>();
                Session["YeuThich"] = lstYeuThich;
            }
            return lstYeuThich;
        }
        public CHITIETSANPHAM YeuThich(int ms)
        {
            CHITIETSANPHAM ct = new CHITIETSANPHAM();
            ct.MaSanPham = ms;
            SANPHAM s = db.SANPHAMs.Single(n => n.MaSanPham == ct.MaSanPham);
            ct.TenSanPham = s.TenSanPham;
            ct.AnhBia = s.AnhBia;
            ct.GiaBan = int.Parse(ct.GiaBan.ToString());
            ct.SoLuong = 1;
            return ct;
        }
        public ActionResult ThemYeuThich(int ms, string url)
        {
            //Lấy giỏ hàng hiện tại
            List<CHITIETSANPHAM> lstYeuThich = LayYeuThich();
            //Kiểm tra nếu sản phẩm chưa có trong giỏ thì thêm vào, nếu đã có thì tăng số lượng
            CHITIETSANPHAM sp = lstYeuThich.Find(n => n.MaSanPham == ms);
            if (sp == null)
            {
                sp = new CHITIETSANPHAM();
                lstYeuThich.Add(sp);
            }
            else
            {
                sp.SoLuong++;
            }
            return Redirect(url);
        }
        //private int TongSoLuong()
        //{
        //    int iTongSoLuong = 0;
        //    List<CHITIETSANPHAM> lstYeuThich = Session["YeuThich"] as List<CHITIETSANPHAM>;
        //    if (lstYeuThich != null)
        //    {
        //        iTongSoLuong = lstYeuThich.Sum(n => n.SoLuong);
        //    }
        //    return iTongSoLuong;
        //}
        //private double TongTien()
        //{
        //    double dTongTien = 0;
        //    List<CHITIETSANPHAM> lstYeuThich = Session["YeuThich"] as List<CHITIETSANPHAM>;
        //    if (lstYeuThich != null)
        //    {
        //        dTongTien = lstYeuThich.Sum(n => n.SoLuong*n.GiaBan);
        //    }
        //    return dTongTien;
        //}
        public ActionResult YeuThich()
        {
            List<CHITIETSANPHAM> lstYeuThich = LayYeuThich();
            if (lstYeuThich.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            //ViewBag.TongSoLuong = TongSoLuong();
            //ViewBag.TongTien = TongTien();
            return View(lstYeuThich);
        }
        public ActionResult YeuThichPartial()
        {
            //ViewBag.TongSoLuong = TongSoLuong();
            //ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSPKhoiYeuThich(int iMaSanPham)
        {
            List<CHITIETSANPHAM> lstYeuThich = LayYeuThich();
            CHITIETSANPHAM sp = lstYeuThich.SingleOrDefault(n => n.MaSanPham == iMaSanPham);
            if (sp != null)
            {
                lstYeuThich.RemoveAll(n => n.MaSanPham == iMaSanPham);
                if (lstYeuThich.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("YeuThich");
        }
        public ActionResult CapNhatYeuThich(int iMaSanPham, FormCollection f)
        {
            List<CHITIETSANPHAM> lstYeuThich = LayYeuThich();
            CHITIETSANPHAM sp = lstYeuThich.SingleOrDefault(n => n.MaSanPham == iMaSanPham);
            if (sp != null)
            {
                sp.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("YeuThich");
        }
        public ActionResult XoaYeuThich()
        {
            List<CHITIETSANPHAM> lstYeuThich = LayYeuThich();
            lstYeuThich.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}