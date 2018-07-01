using SalesAdminPortal.Helpers;
using SalesAdminPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesAdminPortal.Controllers
{
    [Authorize]
    public class SalesCommissionController : ApiController
    {
        [HttpGet]
        [Route("api/commission/{agentCode}")]
        public double Commission(string agentCode)
        {
            using (var context = new ApplicationDbContext())
            {
                double commisionPercent = context.Commissions.Where(r => r.AgentCode == agentCode).Select(r => r.CommissionPercent).FirstOrDefault();
                return commisionPercent;
            }
        }

        [HttpPost]
        [Route("api/commission/")]
        public HttpResponseMessage Commission(AgentCommission commission)
        {
            using(var context = new ApplicationDbContext())
            {
                var entity = context.Commissions.Where(r => r.AgentCode == commission.AgentCode).FirstOrDefault();
                if ((entity!=null) && (entity.CommissionId == commission.CommissionId))
                {
                    entity.CommissionPercent = commission.CommissionPercent;
                }
                else   //New Record
                {
                    commission.CommissionId = entity.CommissionId;
                    context.Commissions.Add(commission);
                }

                return Request.CreateResponse(HttpStatusCode.OK, context.SaveChanges() == 1 ? true : false);
            }
        }

        [HttpGet]
        [Route("api/agentlist/")]
        public HttpResponseMessage AgentList()
        {
            var agentList = new List<string>();
            if (User.Identity.GetAccountType().Equals("M"))
            {
                using(var context = new ApplicationDbContext())
                {
                    agentList = context.Users.Where(r => r.IsMasterAgent).Select(x => x.AgentCode).ToList();
                }
            }
            else if (User.Identity.GetAccountType().Equals("SM"))
            {
                using(var context = new ApplicationDbContext())
                {
                    var currentAgentCode = User.Identity.GetAgentCode();

                    agentList = context.Users.Where(r => r.AgentCode.StartsWith(currentAgentCode)).Select(r => r.AgentCode).ToList();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, agentList);
        }

    }
}
