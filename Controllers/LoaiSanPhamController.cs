using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;
using NguyenChiTrung_ShopYAME.Models;
using PagedList;
using PagedList.Mvc;
namespace NguyenChiTrung_ShopYAME.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        DataShopYameDataContext data = new DataShopYameDataContext();

        public ActionResult LoaiSanPhamPartial()
        {
            var list = from cd in data.LOAISANPHAMs select cd;
            return PartialView(list);
        }

        public ActionResult SanPhamTheoLoai(int id, int? page)
        {
            ViewBag.MaLoaiSanPham =
            id;
            int iSize = 10;
            int iPageNum =
            (page ?? 1);
            var sp = from s in data.SANPHAMs where s.MaLoaiSanPham == id select s;
            return View(sp.ToPagedList(iPageNum, iSize));
        }

























        private List<SANPHAM> AoThun(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 1).Take(count).ToList();
        }

        public ActionResult AoThunPartial()
        {
            var list = AoThun(3);
            return PartialView(list);
        }
        private List<SANPHAM> AoKhoac(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 2).Take(count).ToList();
        }

        public ActionResult AoKhoacPartial()
        {
            var list = AoKhoac(3);
            return PartialView(list);
        }
        private List<SANPHAM> AoSoMi(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 3).Take(count).ToList();
        }

        public ActionResult AoSoMiPartial()
        {
            var list = AoSoMi(3);
            return PartialView(list);
        }
        private List<SANPHAM> QuanDai(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 4).Take(count).ToList();
        }

        public ActionResult QuanDaiPartial()
        {
            var list = QuanDai(3);
            return PartialView(list);
        }
        private List<SANPHAM> QuanShort(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 5).Take(count).ToList();
        }

        public ActionResult QuanShortPartial()
        {
            var list = QuanShort(3);
            return PartialView(list);
        }
        private List<SANPHAM> PhuKien(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 6).Take(count).ToList();
        }

        public ActionResult PhuKienPartial()
        {
            var list = PhuKien(3);
            return PartialView(list);
        }
        private List<SANPHAM> BaloTui(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 7).Take(count).ToList();
        }

        public ActionResult BaloTuiPartial()
        {
            var list = BaloTui(3);
            return PartialView(list);
        }
        private List<SANPHAM> GiayDep(int count)
        {
            return data.SANPHAMs.Where(a => a.MaLoaiSanPham == 8).Take(count).ToList();
        }

        public ActionResult GiayDepPartial()
        {
            var list = GiayDep(3);
            return PartialView(list);
        }
    }
}