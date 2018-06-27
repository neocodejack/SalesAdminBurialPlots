using SalesAdminPortal.Helpers;
using SalesAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        //[Authorize]
        [HttpPost]
        [Route("api/sales/commissionbydate/")]
        public HttpResponseMessage CommissionByDate(DateRange dateRange)
        {
            try
            {
                var ddtStartDate = Convert.ToDateTime(dateRange.StartDate);
                var ddtEndDate = Convert.ToDateTime(dateRange.EndDate);
                var agentCode = User.Identity.GetAgentCode();
                List<SalesTransaction> response = new List<SalesTransaction>();

                using (var context = new ApplicationDbContext())
                {
                    List<SalesTransaction> sales = null;
                    sales = context.SalesTransactions.Where(r => r.AgentCode.StartsWith(agentCode)
                                                                && (r.SaleDate >= ddtStartDate.Date) && (r.SaleDate <= ddtEndDate.Date))
                                                    .ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, sales);
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("api/sales/downloadpdf/")]
        public HttpResponseMessage ExportToPdf()
        {
            try
            {
                Byte[] res = null;

                //var ddtStartDate = Convert.ToDateTime(startDate);
                //var ddtEndDate = Convert.ToDateTime(endDate);
                var agentCode = User.Identity.GetAgentCode();
                string html = "<html><body>Welcome<table><thead><th>Id</th><th>Order Id</th><th>Agent Code</th><th>Selling Price</th><th>Commission</th></thead><tbody>";

                using (var context = new ApplicationDbContext())
                {
                    List<SalesTransaction> sales = null;
                    sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(agentCode))
                                                                //&& (r.SaleDate >= ddtStartDate.Date) && (r.SaleDate <= ddtEndDate.Date))
                                                    .ToList();

                    foreach (var item in sales)
                    {
                        html += "<tr><td>" + item.OrderId + "</td><td>" + item.AgentCode + "</td><td>" + item.PorpSellingPrice + "</td><td>" + item.Commission + "</td></tr>";
                    }

                    html += "</tbody></table></body</html>";
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);

                    pdf.Save(ms);
                    res = ms.ToArray();
                    var response = new HttpResponseMessage
                    {
                        Content = new StreamContent(new MemoryStream(res))
                    };
                    response.Content.Headers.ContentLength = res.Length;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = DateTime.Now.ToString() + User.Identity.GetAgentCode() + ".pdf",
                        Size = pdf.FileSize
                    };

                    return response;
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
