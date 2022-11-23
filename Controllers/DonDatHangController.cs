using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenChiTrung_ShopYAME.Models;

namespace NguyenChiTrung_ShopYAME.Controllers
{
    public class DonDatHangController : Controller
    {
        DataShopYameDataContext data = new DataShopYameDataContext();

        public ActionResult DonHang(int id)
        {

            var resurt = from s in data.DONDATHANGs where s.MaDonHang == id select s;
            return View(resurt);
        }
        public ActionResult ChiTietDonHang(int id)
        {

            var resurt = from s in data.CHITIETDONDATHANGs where s.MaDonHang == id select s;
            return View(resurt);
        }
        public ActionResult KhachHang()
        {

            return PartialView();
        }
    }
}