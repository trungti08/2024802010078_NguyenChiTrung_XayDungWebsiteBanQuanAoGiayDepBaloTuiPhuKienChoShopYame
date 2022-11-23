using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using NguyenChiTrung_ShopYAME.Models;
using PagedList;
using PagedList.Mvc;

namespace NguyenChiTrung_ShopYAME.Controllers
{
    public class HomeController : Controller
    {
        DataShopYameDataContext data = new DataShopYameDataContext();

        private List<SANPHAM> LaySpMoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int iSize = 3;
            int iPageNum =(page ?? 1);
            var listSpMoi = LaySpMoi(6);
 



            return View(listSpMoi.ToPagedList(iPageNum, iSize));
        }
        public ActionResult KhuyenMai()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult ChiTietSanPham(int id)
        {
            var sp = from s in data.SANPHAMs where s.MaSanPham == id select s;
            return View(sp.Single());
        }
        public ActionResult HienThiComment(int id)
        {
            var sp = from s in data.CHITIETSANPHAMs where s.MaSanPham == id select s;
            return PartialView(sp);
        }
        [HttpGet]
        public ActionResult GiaKM(int id)
        {
            var sp = from s in data.KHUYENMAIs where s.MaSanPham == id select s;
            return PartialView(sp);
        }
        public ActionResult Comment()
        {
           // var sp = from s in data.CHITIETSANPHAMs where s.MaSanPham == id select s;
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Comment(FormCollection f,CHITIETSANPHAM ct)
        {
            var sHoTen = f["sHoTen"];
            var sComment =
            f["sComment"];
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err4"] = "Phải nhập lại họ tên";
            }
            else if (String.IsNullOrEmpty(sComment))
            {
                ViewData["err4"] = "Phải nhập bình luận";
            }
            else
            {
                ct.HoTen = sHoTen;
                ct.Comment = sComment;
               // ct.MaSanPham= id;
                data.CHITIETSANPHAMs.InsertOnSubmit(ct);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Hinh(int id)
        {
            var sp = from s in data.DANHMUCHINHs where s.MaSanPham == id select s;
            return PartialView(sp);
        }
    }
}