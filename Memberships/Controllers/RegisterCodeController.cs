using Memberships.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Memberships.Controllers
{
    public class RegisterCodeController : Controller
    {
        // GET: RegisterCode
        public async Task<ActionResult> Register(string code)
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                var registered = await SubscriptionExtension.RegisterUserSubscriptionCode(
                    userId, code);

                if (!registered)
                {
                    throw new ApplicationException();
                }

                return PartialView("_RegisterCodePartial");
            }

            return View();
        }
    }
}