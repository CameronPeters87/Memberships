using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Memberships.Entities;
using Memberships.Models;
using Memberships.Areas.Admin.Models;
using Memberships.Areas.Admin.Extensions;

namespace Memberships.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubscriptionProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SubscriptionProducts
        public async Task<ActionResult> Index()
        {
            var model = await db.SubscriptionProducts.Convert(db);

            return View(model);
        }

        // GET: Admin/SubscriptionProducts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await db.SubscriptionProducts.FindAsync(id);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionProduct);
        }

        // GET: Admin/SubscriptionProducts/Create
        public async Task<ActionResult> Create()
        {
            var model = new SubscriptionProductModel
            {
                Products = await db.Products.ToListAsync(),
                Subscriptions = await db.Subscriptions.ToListAsync()
            };
            return View(model);
        }

        // POST: Admin/SubscriptionProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,SubscriptionId")] SubscriptionProduct subscriptionProduct)
        {
            if (ModelState.IsValid)
            {
                db.SubscriptionProducts.Add(subscriptionProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subscriptionProduct);
        }

        // GET: Admin/SubscriptionProducts/Edit/5
        public async Task<ActionResult> Edit(int? id, int? subscriptionId, int? productId)
        {
            if (subscriptionId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //SubscriptionProduct subscriptionProduct = await db.SubscriptionProducts.FindAsync(id);
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct
                (subscriptionId, productId);

            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }
            return View(await subscriptionProduct.Convert(db));
        }

        private async Task<SubscriptionProduct> GetSubscriptionProduct
            (int? subscriptionId, int? productId)
        {
            try
            {
                int prdId = 0;
                int subId = 0;

                // Assign itmId and prdId to the parameters
                int.TryParse(productId.ToString(), out prdId);
                int.TryParse(subscriptionId.ToString(), out subId);

                // use those 2 ids to fetch the correct productItem
                var subscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(
                    sp => sp.ProductId.Equals(prdId) && sp.SubscriptionId.Equals(subId));

                return subscriptionProduct;
            }
            catch
            {
                return null;
            }
        }


        // POST: Admin/SubscriptionProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,SubscriptionId")] SubscriptionProduct subscriptionProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriptionProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subscriptionProduct);
        }

        // GET: Admin/SubscriptionProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await db.SubscriptionProducts.FindAsync(id);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionProduct);
        }

        // POST: Admin/SubscriptionProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubscriptionProduct subscriptionProduct = await db.SubscriptionProducts.FindAsync(id);
            db.SubscriptionProducts.Remove(subscriptionProduct);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
