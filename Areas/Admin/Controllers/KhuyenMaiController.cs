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
    public class KhuyenMaiController : Controller
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

            return View(db.KHUYENMAIs.ToList().OrderByDescending(n => n.MaKM).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaSanPham = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSanPham), "MaSanPham", "TenSanPham");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHUYENMAI sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //Đưa dữ liệu vào DropDown
            ViewBag.MaSanPham = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSanPham), "MaSanPham", "TenSanPham");
            if (fFileUpload == null)
            {
                //Nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
                ViewBag.GiaTriKM = decimal.Parse(f["GiaTriKM"]);


                ViewBag.MaSanPham = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSanPham), "MaSanPham", "TenSanPham", int.Parse(f["MaSanPham"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/img/KM"), sFileName);
                    //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    //Lưu Sạch vào CSDL

                    sach.HinhKM = sFileName;

                    sach.GiaTriKM= decimal.Parse(f["GiaTriKM"]);
                    sach.MaSanPham = int.Parse(f["MaSanPham"]);

                    db.KHUYENMAIs.InsertOnSubmit(sach);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var sach = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == id);
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
            var sach = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == id);
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
            var sach = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.KHUYENMAIs.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaSanPham = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSanPham), "MaSanPham", "TenSanPham");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var sach = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == int.Parse(f["MaKM"]));
            ViewBag.MaSanPham = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSanPham), "MaSanPham", "TenSanPham");
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/img/KM"), sFileName);

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.HinhKM = sFileName;
                }

                sach.GiaTriKM = decimal.Parse(f["GiaTriKM"]);



                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}