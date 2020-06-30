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
    public class ProductItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductItems
        public async Task<ActionResult> Index()
        {
            var model = await db.ProductItems.Convert(db);
            
            return View(model);
        }

        // GET: Admin/ProductItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await db.ProductItems.FindAsync(id);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(productItem);
        }

        // GET: Admin/ProductItems/Create
        public async Task<ActionResult> Create()
        {
            var model = new ProductItemModel
            {
                Products = await db.Products.ToListAsync(),
                Items = await db.Items.ToListAsync()
            };
            return View(model);
        }

        // POST: Admin/ProductItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,ItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                db.ProductItems.Add(productItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productItem);
        }

        // GET: Admin/ProductItems/Edit/5
        public async Task<ActionResult> Edit(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ProductItem productItem = await db.ProductItems.FindAsync(id);
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.Convert(db));
        }

        // POST: Admin/ProductItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,ItemId,OldProductId,OldItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                var canChange = await productItem.CanChange(db);
                if (canChange)
                    await productItem.Change(db);
                return RedirectToAction("Index");
            }
            return View(productItem);
        }

        private async Task<ProductItem> GetProductItem (int? itemId, int? productId)
        {
            try
            {
                int prdId = 0;
                int itmId = 0;

                // Assign itmId and prdId to the parameters
                int.TryParse(productId.ToString(), out prdId);
                int.TryParse(itemId.ToString(), out itmId);

                // use those 2 ids to fetch the correct productItem
                var productItem = await db.ProductItems.FirstOrDefaultAsync(
                    pi => pi.ProductId.Equals(prdId) && pi.ItemId.Equals(itmId));

                return productItem;
            }
            catch
            {
                return null;
            }
        }

        // GET: Admin/ProductItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await db.ProductItems.FindAsync(id);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(productItem);
        }

        // POST: Admin/ProductItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductItem productItem = await db.ProductItems.FindAsync(id);
            db.ProductItems.Remove(productItem);
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
