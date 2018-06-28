using SalesAdminPortal.Helpers;
using SalesAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                var ddtStartDate = Convert.ToDateTime(dateRange.StartDate, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat);
                var ddtEndDate = Convert.ToDateTime(dateRange.EndDate, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat);
                var agentCode = User.Identity.GetAgentCode();
                List<SalesTransaction> response = new List<SalesTransaction>();

                using (var context = new ApplicationDbContext())
                {
                    List<SalesTransaction> sales = null;
                    
                    if (agentCode.Contains('-'))
                    {
                        sales = context.SalesTransactions.Where(r => r.AgentCode.StartsWith(agentCode))
                                                        //&& (r.SaleDate >= ddtStartDate.Date) && (r.SaleDate <= ddtEndDate.Date))
                                                        .ToList();
                    }
                    else
                    {
                        sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(agentCode))
                                                        //&& (r.SaleDate >= ddtStartDate.Date) && (r.SaleDate <= ddtEndDate.Date))
                                                        .ToList();
                    }

                    foreach(var item in sales)
                    {
                        if((item.SaleDate.Date>=ddtStartDate.Date) && (item.SaleDate.Date <= ddtEndDate.Date))
                        {
                            response.Add(item);
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("api/sales/downloadpdf/")]
        public HttpResponseMessage ExportToPdf(DateRange dateRange)
        {
            try
            {
                Byte[] res = null;

                var ddtStartDate = Convert.ToDateTime(dateRange.StartDate, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat);
                var ddtEndDate = Convert.ToDateTime(dateRange.EndDate, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat);
                var agentCode = User.Identity.GetAgentCode();
                
                string html = "<html><style type=\"text/css\">table, th, td {border: 1px solid black;border-collapse: collapse;}</style><body><div><img src=\""+ConfigurationManager.AppSettings["imgCdn"].ToString()+"/Images/logo.png\" /></div><br/><div>Burial Plots Sales Report<div><br/><div>From Date: " 
                                                            + ddtStartDate.Date + "</div><br/><div>To Date: " + ddtEndDate.Date +  
                                                            "<br/><br/><br/><table style=\"border:1; width: 100%\"><tbody><tr><td>Order Id</td><td>Agent Code</td><td>Selling Price</td><td>Commission</td></tr>";

                using (var context = new ApplicationDbContext())
                {
                    List<SalesTransaction> sales = null;
                    double totalCommission = 0;

                    sales = context.SalesTransactions.Where(r => r.AgentCode.Equals(agentCode)
                                                                && (r.SaleDate >= ddtStartDate.Date) && (r.SaleDate <= ddtEndDate.Date))
                                                    .ToList();

                    foreach (var item in sales)
                    {
                        html += "<tr><td>" + item.OrderId + "</td><td>" + item.AgentCode + "</td><td>&pound; " + item.PorpSellingPrice + "</td><td>&pound; " + item.Commission + "</td></tr>";
                        totalCommission += Convert.ToDouble(item.Commission);
                    }

                    html += "</tbody></table><br/><div>Total Commission: &pound; " + totalCommission + "</div></body</html>";
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
