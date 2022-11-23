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

namespace NguyenChiTrung_ShopYAME.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
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
            return View(db.KHACHHANGs.ToList().OrderByDescending(n => n.MaKhachHang).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHACHHANG sach, FormCollection f)
        {
            if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sach.TaiKhoan) != null)
            {
                ViewData["err4"] = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sach.Email) != null)
            {
                ViewData["err4"] = "Email đã được sử dụng";
            }
            else
            {
                sach.HoTen = f["sHoTen"];
                sach.TaiKhoan = f["sTaiKhoan"];
                sach.MatKhau = f["sMatKhau"];
                sach.Email = f["sEmail"];
                sach.DiaChi = f["sDiaChi"];
                sach.DienThoai = f["sDienThoai"];

                sach.NgaySinh = Convert.ToDateTime(f["dNgaySinh"]);


                db.KHACHHANGs.InsertOnSubmit(sach);
                db.SubmitChanges();

                return RedirectToAction("Index");
            }
            return this.Create();
        }
        public ActionResult Details(int id)
        {
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == id);
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
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == id);
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
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.DONDATHANGs.Where(ct => ct.MaKhachHang == id);
            if (ctdh.Count() > 0)
            {
                //Nội dung sẽ hiển thị khi sách cần xóa đã có trong table ChiTietDonHang
                ViewBag.ThongBao = "Khách hàng này đang có trong bảng đặt hàng <br>" +
                " Nếu muốn xóa thì phải xóa hết mã khách hàng này trong bảng đặt hàng";
                return View(sach);
            }
            db.KHACHHANGs.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Hiển thị danh sách chủ đề và nhà xuất bản đồng thời chọn chủ đề và nhà xuất bản của cuốn hiện tại
            
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == int.Parse(f["iMaKhachHang"]));
           
            if (ModelState.IsValid)
            {

                sach.HoTen = f["sHoTen"];
                sach.TaiKhoan = f["sTaiKhoan"];
                sach.MatKhau = f["sMatKhau"];
                sach.Email = f["sEmail"];
                sach.DiaChi = f["sDiaChi"];
                sach.DienThoai = f["sDienThoai"];

                sach.NgaySinh = Convert.ToDateTime(f["dNgaySinh"]);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}