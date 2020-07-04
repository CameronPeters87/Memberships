using Memberships.Comparer;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Memberships.Extensions
{
    public static class ThumbnailExtension
    {
        // Get list of all SubIDs that user has
        public static async Task<List<int>> GetSubscriptionIdAsync(
            string userId = null, ApplicationDbContext db = null)
        {
            try
            {
                if (userId == null) return new List<int>();
                if (db == null)
                    db = ApplicationDbContext.Create();

                return await (from us in db.UserSubscriptions // Get UserSub table 
                              where us.UserId.Equals(userId) // Table UserId == userId parameter
                              select us.SubscriptionId).ToListAsync(); // Get list of SubIds
            }
            catch { }

            return new List<int>();
        }

        // Use this method to get ProductThumbnails in index View
        public static async Task<IEnumerable<ThumbnailModel>> GetProductThumbnailsAsync (
            this List<ThumbnailModel> thumbnails, string userId = null,
            ApplicationDbContext db = null)
        {
            try 
            {
                if (userId == null) return new List<ThumbnailModel>();
                if (db == null)
                    db = ApplicationDbContext.Create();

                // Get all subIds user has using extension method above
                var subscriptionIds = await GetSubscriptionIdAsync(userId, db);

                /* 
                    List<ThumbnailModel> thumbnails = new Lists<ThumbnailModel>()
                    We gonna populate viewmodel
                    we using SP, P, PLT, PT tables from db
                    Need to join them and see if ProductId equals all in tables
                    Where subIds equal SP.subsIds
                    Select New ThumbnailVM { populate and assign }
                 */
                thumbnails = await
                    (from sp in db.SubscriptionProducts
                     join p in db.Products on sp.ProductId equals p.Id
                     join plt in db.ProductLinkTexts on p.ProductLinkTextId equals plt.Id
                     join pt in db.ProductTypes on p.ProductTypeId equals pt.Id
                     where subscriptionIds.Contains(sp.SubscriptionId)
                     select new ThumbnailModel
                     {
                         ProductId = p.Id,
                         SubscriptionId = sp.SubscriptionId,
                         Title = p.Title,
                         Description = p.Description,
                         ImageUrl = p.ImageUrl,
                         Link = "/ProductContent/Index/" + p.Id,
                         ContentTag = pt.Title,
                         TagText = plt.Title
                     }).ToListAsync();
            }
            catch { }

            /*
             * Return thumbnails that are not the same 
             * using the comparer method and 
             * order by Title 
             */
            return thumbnails.Distinct(new ThumbnailEqualityComparer())
                .OrderBy(o => o.Title);
        }
    }
}