using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenChiTrung_ShopYAME.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.Helpers;
using System.Web.UI.WebControls;

namespace NguyenChiTrung_ShopYAME.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        DataShopYameDataContext db = new DataShopYameDataContext();
        // GET: Admin/Sach
        public ActionResult Index(int? page)
        {
            if (Session["TenDangNhap"] == null || Session["TenDangNhap"].ToString() == "")
            {
                return RedirectToAction("Login", "TrangChu");
            }

            int iPageNum = (page ?? 1);
            int iPageSize = 3;
            return View(db.DONDATHANGs.ToList().OrderByDescending(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaKhachHang = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKhachHang", "HoTen");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DONDATHANG sach, FormCollection f)
        {
            ViewBag.MaKhachHang = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKhachHang", "HoTen");
            //sach.DaThanhToan = bool.Parse(f["sDaThanhToan"]);
            sach.TinhTrangGiaoHang = int.Parse(f["sTinhTrangGiaoHang"]);
            sach.NgayDat = Convert.ToDateTime(f["sNgayDat"]);
                sach.NgayGiao = Convert.ToDateTime(f["sNgayGiao"]);
            sach.MaKhachHang = int.Parse(f["MaKhachHang"]);
            db.DONDATHANGs.InsertOnSubmit(sach);
                db.SubmitChanges();
                return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.DONDATHANGs.Where(ct => ct.MaDonHang == id);
            if (ctdh.Count() > 0)
            {
                //Nội dung sẽ hiển thị khi sách cần xóa đã có trong table ChiTietDonHang
                ViewBag.ThongBao = "Don hàng này đang có trong bảng chi tiet đặt hàng <br>" +
                " Nếu muốn xóa thì phải xóa hết mã don hang này trong bảng chi tiet đặt hàng";
                return View(sach);
            }
            db.DONDATHANGs.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Hiển thị danh sách chủ đề và nhà xuất bản đồng thời chọn chủ đề và nhà xuất bản của cuốn hiện tại
            ViewBag.MaKhachHang = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKhachHang", "HoTen");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == int.Parse(f["sMaDonHang"]));
            ViewBag.MaKhachHang = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKhachHang", "HoTen");
            if (ModelState.IsValid)
            {

                sach.DaThanhToan = bool.Parse(f["sDaThanhToan"]);
                sach.TinhTrangGiaoHang = int.Parse(f["sTinhTrangGiaoHang"]);
                sach.NgayDat = Convert.ToDateTime(f["sNgayDat"]);
                sach.NgayGiao = Convert.ToDateTime(f["sNgayGiao"]);
                
                sach.MaKhachHang = int.Parse(f["MaKhachHang"]);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}