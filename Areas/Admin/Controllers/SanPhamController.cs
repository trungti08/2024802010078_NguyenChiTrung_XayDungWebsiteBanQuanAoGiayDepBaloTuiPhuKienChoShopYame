using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenChiTrung_ShopYAME.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace NguyenChiTrung_ShopYAME.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        DataShopYameDataContext db = new DataShopYameDataContext();
        // GET: Admin/Sach
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 3;
            if (Session["TenDangNhap"] == null || Session["TenDangNhap"].ToString() == "")
            {
                return RedirectToAction("Login", "TrangChu");
            }

            return View(db.SANPHAMs.ToList().OrderByDescending(n => n.MaSanPham).ToPagedList(iPageNum, iPageSize));
            }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoaiSanPham = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.TenLoaiSanPham), "MaLoaiSanPham", "TenLoaiSanPham");
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SANPHAM sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //Đưa dữ liệu vào DropDown
            ViewBag.MaLoaiSanPham = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.TenLoaiSanPham), "MaLoaiSanPham", "TenLoaiSanPham");
            if (fFileUpload == null)
            {
                //Nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.ChatLieu = f["sChatLieu"];
                ViewBag.KichThuoc = f["sKichThuoc"];
                ViewBag.MauSac = f["sMauSac"];
                ViewBag.TenChiTietLoaiSanPham = f["sTenChiTietLoaiSanPham"];
              
                ViewBag.MaLoaiSanPham = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n =>
                n.TenLoaiSanPham), "MaLoaiSanPham", "TenLoaiSanPham", int.Parse(f["MaLoaiSanPham"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/img"), sFileName);
                    //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    //Lưu Sạch vào CSDL
                    sach.ChatLieu = f["sChatLieu"];
                    sach.KichThuoc = f["sKichThuoc"];
                    sach.MauSac = f["sMauSac"];
                    sach.TenChiTietLoaiSanPham = f["sTenChiTietLoaiSanPham"];
                    sach.TenSanPham = f["sTenSach"];
                    sach.MoTa = f["sMoTa"];
                    sach.AnhBia = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                     sach.MaLoaiSanPham = int.Parse(f["MaLoaiSanPham"]);
                  
                    db.SANPHAMs.InsertOnSubmit(sach);
                    db.SubmitChanges();
                   
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var sach = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
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
            var sach = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
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
            var sach = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDONDATHANGs.Where(ct => ct.MaSanPham == id);
            if (ctdh.Count() > 0)
            {
                //Nội dung sẽ hiển thị khi sách cần xóa đã có trong table ChiTietDonHang
                ViewBag.ThongBao = "Sách này đang có trong bảng Chi tiết đặt hàng <br>" +
                " Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";
                return View(sach);
            }
            //Xóa hết thông tin của cuốn sách trong table VietSach trước khi xóa sách này
            //var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            //if (vietsach != null)
            //{
            //    db.VIETSACHes.DeleteAllOnSubmit(vietsach);
            //    db.SubmitChanges();
            //}
            db.SANPHAMs.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Hiển thị danh sách chủ đề và nhà xuất bản đồng thời chọn chủ đề và nhà xuất bản của cuốn hiện tại
            ViewBag.MaLoaiSanPham = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.TenLoaiSanPham), "MaLoaiSanPham", "TenLoaiSanPham");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var sach = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == int.Parse(f["iMaSanPham"]));
            ViewBag.MaLoaiSanPham = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.TenLoaiSanPham), "MaLoaiSanPham", "TenLoaiSanPham");
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/img"), sFileName);

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                }
                sach.TenSanPham = f["sTenSanPham"];
                sach.MoTa = f["sMoTa"];
                sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sach.MaLoaiSanPham = int.Parse(f["MaLoaiSanPham"]);
                sach.ChatLieu = f["sChatLieu"];
                sach.KichThuoc = f["sKichThuoc"];
                sach.MauSac = f["sMauSac"];
            
                sach.TenChiTietLoaiSanPham = f["sTenChiTietLoaiSanPham"];
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}