using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesAdminPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            try
            {
                var client = new HttpClient();
                var response = client.GetAsync("http://185.197.61.184/salesadmin/api/coupon").Result;
                var coupon = response.Content.ReadAsStringAsync();
                ViewBag.Coupon = coupon;
                
            }catch(Exception ex)
            {
                logger.Error(ex);
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}