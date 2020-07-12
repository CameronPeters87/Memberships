using Memberships.Entities;
using Memberships.Extensions;
using Memberships.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Controllers
{
    public class ProductContentController : Controller
    {
        // GET: ProductContent
        public async Task<ActionResult> Index(int id)
        {
            //string userId = Request.IsAuthenticated ? HttpContext.GetUserId() : null;
            string userId = User.Identity.GetUserId();
            var sections = SectionsExtension.GetProductSections(id, userId);

            return View(await sections);
        }
    }
}