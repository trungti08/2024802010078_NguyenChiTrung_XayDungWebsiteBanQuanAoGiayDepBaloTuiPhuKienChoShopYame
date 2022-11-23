using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NguyenChiTrung_ShopYAME.Areas.Admin.Controllers;

namespace NguyenChiTrung_ShopYAME
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Trang chủ",
            url: "",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,

            }
            //Thêm hàng sau để tránh xung đột giữa các controller Home
            , namespaces: new[] { "Home.Controllers" }
            );
            routes.MapRoute(
                name: "Trang tin",
                url: "{metatitle}",
                defaults: new
                {
                    controller = "Home",
                    action = "TrangTin",
                    metatitle = UrlParameter.Optional
                },
                namespaces: new string[] { "Home.Controllers" }
                );

            routes.MapRoute(
            name: "Login",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "User",
                action = "Login",
                id = UrlParameter.Optional,
            }
            //Thêm hàng sau để tránh xung đột giữa các controller Home
            , namespaces: new[] { "Home.Controllers" }
            );

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,

            }
            //Thêm hàng sau để tránh xung đột giữa các controller Home
            , namespaces: new[] { "Home.Controllers" }
            );
        }
    }
}
