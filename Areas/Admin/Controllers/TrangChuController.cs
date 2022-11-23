using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenChiTrung_ShopYAME.Models;
using System.Web.Security;
namespace NguyenChiTrung_ShopYAME.Areas.Admin.Controllers
{
    public class TrangChuController : Controller
    {
        DataShopYameDataContext db = new DataShopYameDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var sTenDangNhap = collection["TenDangNhap"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDangNhap))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }

            else
            {
                ADMIN kh = db.ADMINs.SingleOrDefault(n => n.TenDangNhap == sTenDangNhap && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TenDangNhap"] = kh;
                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDangNhap"].Value = sTenDangNhap;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TenDangNhap"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["TenDangNhap"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                    Session["HoTen"] = kh.HoTen;
                    return RedirectToAction("Index", "TrangChu");

                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";

                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TenDangNhap"] = null;
            Session["HoTen"] = null;
            return RedirectToAction("Index", "TrangChu");
        }
    }
}