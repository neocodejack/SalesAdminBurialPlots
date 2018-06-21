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
                    
                    var objSaleTransaction = new SalesTransaction { AgentCode = sale.AgentCode, OrderId = sale.OrderId, PorpSellingPrice = sale.SellingPrice, Commission = commission.ToString(), SaleDate = DateTime.Now };
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

        [Authorize]
        [HttpGet]
        [Route("api/sales/order/{id}")]
        public HttpResponseMessage GetByOrderId(string id)
        {
            using(var context = new ApplicationDbContext())
            {
                List<SalesTransaction> sales = null;
                var currentAgentCode = User.Identity.GetAgentCode();
                if (User.Identity.GetAccountType().Equals("A"))
                {
                    sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(currentAgentCode) && r.OrderId.Equals(id)).ToList();
                }
                else
                {
                    sales = context.SalesTransactions.Where(r => r.AgentCode.StartsWith(currentAgentCode) && r.OrderId.Equals(id)).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, sales);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales/agentcode/{code}")]
        public HttpResponseMessage GetByAgentCode(string code)
        {
            using(var context = new ApplicationDbContext())
            {
                List<SalesTransaction> sales = null;
                if (User.Identity.GetAccountType().Equals("SM"))
                {
                    sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(code)).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, sales);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales/{startDate}/{endDate}")]
        public HttpResponseMessage GetCommissionByDate(string startDate, string endDate)
        {
            var ddtStartDate = Convert.ToDateTime(startDate);
            var ddtEndDate = Convert.ToDateTime(endDate);

            using(var context = new ApplicationDbContext())
            {
                List<SalesTransaction> sales = null;
                sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(User.Identity.GetAgentCode()) 
                                                            && (r.SaleDate >= ddtStartDate) && (r.SaleDate <= ddtEndDate))
                                                .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, sales);
            }
        }
    }
}
