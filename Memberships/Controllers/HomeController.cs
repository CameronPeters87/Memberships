using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Memberships.Extensions;
using System.Threading.Tasks;

namespace Memberships.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            // From HttpContextExtension
            //string userId = Request.IsAuthenticated ? HttpContext.GetUserId() : null;
            string userId = "b7f8396d-e83d-49e2-9b0b-44e63d9b6ba4";
            var thumbnail = await new List<ThumbnailModel>().GetProductThumbnailsAsync(userId);
            var count = thumbnail.Count() / 4;
            var model = new List<ThumbnailAreaModel>();

            for (int i = 0; i <= count; i++)
            {
                model.Add(new ThumbnailAreaModel
                {
                    Title = i.Equals(0) ? "My Content" : string.Empty,
                    Thumbnails = thumbnail.Skip(i * 4).Take(4)
                });
            }
            return View(model);
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