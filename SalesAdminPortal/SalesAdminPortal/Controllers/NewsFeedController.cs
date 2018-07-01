using SalesAdminPortal.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SalesAdminPortal.Controllers
{
    [Authorize]
    public class NewsFeedController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new Feed());
        }

        [HttpPost]
        public ActionResult Index(Feed feed)
        {
            if (ModelState.IsValid)
            {
                using(var context = new ApplicationDbContext())
                {
                    context.DashboardFeeds.Add(new DashboardFeed { Description = feed.Description, Name = feed.Name, IsPublished = true, PublishDate = DateTime.Now });
                    context.SaveChanges();
                }

                ViewBag.Status = "News Published to Portal";
            }
            
            return View(feed);
        }

        [HttpGet]
        public ActionResult ViewFeeds()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PublishedFeeds()
        {
            using (var context = new ApplicationDbContext())
            {
                var feeds = context.DashboardFeeds.Where(r => r.IsPublished).ToList();
                return Json(feeds, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ListAllFeeds()
        {
            using (var context = new ApplicationDbContext())
            {
                var feeds = context.DashboardFeeds.ToList();
                return Json(feeds, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public ActionResult DeleteFeed(int feedId)
        {
            using(var context = new ApplicationDbContext())
            {
                var entity = context.DashboardFeeds.Find(feedId);
                if (entity.IsPublished)
                    entity.IsPublished = false;
                else
                    entity.IsPublished = true;
                return Json(context.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }

    }
}