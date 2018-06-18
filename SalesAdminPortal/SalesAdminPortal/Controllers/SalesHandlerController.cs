using SalesAdminPortal.Helpers;
using SalesAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SalesAdminPortal.Controllers
{
    public class SalesHandlerController : ApiController
    {
        [HttpPost]
        [Route("api/sales/")]
        public HttpResponseMessage Post(Sale sale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var commission = (Convert.ToDouble(sale.SellingPrice) * 20) / 100;
                    var objSaleTransaction = new SalesTransaction { AgentCode = sale.AgentCode, OrderId = sale.OrderId, PorpSellingPrice = sale.SellingPrice, Commission = commission.ToString() };
                    using (var context = new ApplicationDbContext())
                    {
                        if (context.Users.Where(r => r.AgentCode.Equals(sale.AgentCode)).FirstOrDefault() != null)
                        {
                            context.SalesTransactions.Add(objSaleTransaction);
                            context.SaveChanges();
                            return new HttpResponseMessage(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Agent Code Doesn't Exists");
                        }
                    } 
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales")]
        public HttpResponseMessage Get()
        {
            using(var context = new ApplicationDbContext())
            {
                List<SalesTransaction> salesTransaction = null;
                var currentAgentCode = User.Identity.GetAgentCode();
                if (User.Identity.GetAccountType().Equals("A")) //An Agent
                {
                    salesTransaction = context.SalesTransactions.Where(r => r.AgentCode.Equals(currentAgentCode)).ToList();
                }
                else
                {
                    //Get All the records for agents under the same master agent
                    salesTransaction = context.SalesTransactions.Where(r => r.AgentCode.StartsWith(currentAgentCode)).ToList();
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, salesTransaction);
            }
        }
    }
}
