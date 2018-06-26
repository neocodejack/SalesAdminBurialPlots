using SalesAdminPortal.Helpers;
using SalesAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
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

        public JsonResult CommissionByDate(string startDate, string endDate)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime ddtStartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", culture);
            DateTime ddtEndDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", culture);
            //var ddtStartDate = Convert.ToDateTime(startDate);
            //var ddtEndDate = Convert.ToDateTime(endDate);
            var agentCode = User.Identity.GetAgentCode();
            using (var context = new ApplicationDbContext())
            {
                List<SalesTransaction> sales = null;
                sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(agentCode)
                                                            && (DbFunctions.TruncateTime(r.SaleDate) >= ddtStartDate.Date) && (DbFunctions.TruncateTime(r.SaleDate) <= ddtEndDate.Date))
                                                .ToList();

                return Json(sales, JsonRequestBehavior.AllowGet);
            }
        }
    }
}