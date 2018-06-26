using SalesAdminPortal.Models;
using System.Linq;
using System.Web.Mvc;

namespace SalesAdminPortal.Controllers
{
    public class NewsFeedController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new DashboardFeed());
        }

        [HttpPost]
        public ActionResult Index(DashboardFeed feed)
        {
            if (ModelState.IsValid)
            {
                using(var context = new ApplicationDbContext())
                {
                    context.DashboardFeeds.Add(new DashboardFeed { Description = feed.Description, Name = feed.Name });
                    context.SaveChanges();
                }

                return RedirectToAction("ViewFeeds", "NewsFeed");
            }
            
            return View(feed);
        }

        [HttpGet]
        public ActionResult ViewFeeds()
        {
            using (var context = new ApplicationDbContext())
            {
                var feeds = context.DashboardFeeds.Where(r => !r.IsDeleted).ToList();
                return Json(feeds, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public ActionResult DeleteFeed(int feedId)
        {
            using(var context = new ApplicationDbContext())
            {
                var entity = context.DashboardFeeds.Find(feedId);
                entity.IsDeleted = true;
                return Json(context.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}