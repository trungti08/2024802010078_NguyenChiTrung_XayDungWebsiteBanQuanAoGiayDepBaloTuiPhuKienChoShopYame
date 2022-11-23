using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;
using NguyenChiTrung_ShopYAME.Models;
using PagedList;
using PagedList.Mvc;
using System;

namespace NguyenChiTrung_ShopYAME.Controllers
{
    public class SearchController : Controller
    {
        DataShopYameDataContext data = new DataShopYameDataContext();
        public ActionResult Search(string strSearch)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {

                var resurt = from s in data.SANPHAMs where s.TenSanPham.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
                return View(resurt);
            }
            return View();
        }

    }
}