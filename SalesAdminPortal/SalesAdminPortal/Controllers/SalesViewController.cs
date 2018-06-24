using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesAdminPortal.Controllers
{
    [Authorize]
    public class SalesViewController : Controller
    {
        // GET: SalesView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Commission()
        {
            return View();
        }
    }
}