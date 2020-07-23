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
    public static class SectionsExtension
    {
        public static async Task<ProductSectionModel> GetProductSections (
            int productId, string userId)
        {
            var db = ApplicationDbContext.Create();

            var sections = await (from p in db.Products
                                  join pi in db.ProductItems on p.Id equals pi.ProductId
                                  join i in db.Items on pi.ItemId equals i.Id
                                  join s in db.Sections on i.SectionId equals s.Id
                                  where p.Id.Equals(productId)
                                  orderby s.Id
                                  select new ProductSection
                                  {
                                      Id = s.Id,
                                      Title = s.Title,
                                      ItemTypeId = i.ItemTypeId
                                  }).ToListAsync();

            foreach(var section in sections)
            {
                section.Items = await GetProductItemRowAsync(productId, section.Id,
                    section.ItemTypeId, userId);
            }

            var result = sections.Distinct(new ProductSectionEqualityComparer()).ToList();

            // Display downloads at the bottom by getting all sections except downloads
            var union = result.Where(r => !r.Title.ToLower().Contains("download"))
                .Union(result.Where(r => r.Title.ToLower().Contains("download")));

            // Put the data in a the viewModel
            var model = new ProductSectionModel
            {
                Sections = union.ToList(),
                Title = await (from p in db.Products
                               where p.Id.Equals(productId)
                               select p.Title).FirstOrDefaultAsync()
            };

            return model;
        }

        public static async Task<IEnumerable<ProductItemRow>> GetProductItemRowAsync(
            int productId, int sectionId, int itemTypeId, string userId,
            ApplicationDbContext db = null)
        {
            if (db == null)
            {
                db = ApplicationDbContext.Create();
            }

            var today = DateTime.Now.Date;

            var items = await (from i in db.Items
                               join it in db.ItemTypes on i.ItemTypeId equals it.Id
                               join pi in db.ProductItems on i.Id equals pi.ItemId
                               join sp in db.SubscriptionProducts on pi.ProductId equals sp.ProductId
                               join us in db.UserSubscriptions on sp.SubscriptionId equals us.SubscriptionId
                               where pi.ProductId.Equals(productId) &&
                               i.SectionId.Equals(sectionId) &&
                               i.ItemTypeId.Equals(itemTypeId) &&
                               us.UserId.Equals(userId)
                               select new ProductItemRow
                               {
                                   ItemId = i.Id,
                                   Description = i.Description,
                                   Title = i.Title,
                                   Link = it.Title.Equals("Download") ? i.Url : "/ProductContent/Content/" + pi.ProductId + "/" + i.Id,
                                   ImageUrl = i.ImageUrl,
                                   ReleaseDate = DbFunctions.CreateDateTime(us.StartDate.Value.Year,
                                   us.StartDate.Value.Month, us.StartDate.Value.Day + i.WaitDays,
                                   0, 0, 0),
                                   isAvailable = DbFunctions.CreateDateTime(today.Year, today.Month,
                                   today.Day, 0, 0, 0) >= DbFunctions.CreateDateTime(us.StartDate.Value.Year,
                                   us.StartDate.Value.Month, us.StartDate.Value.Day + i.WaitDays,
                                   0, 0, 0),
                                   isDownload = it.Title.Equals("Download") ? true : false
                               }).ToListAsync();
            return items;
        }

        public static async Task<ContentViewModel> GetContentAsync (
            int productId, int itemId)
        {
            var db = ApplicationDbContext.Create();

            return await (from i in db.Items
                          join it in db.ItemTypes on i.ItemTypeId equals it.Id
                          where i.Id.Equals(itemId)
                          select new ContentViewModel
                          {
                              ProductId = productId,
                              HTML = i.HTML,
                              VideoUrl = i.Url,
                              Title = i.Title,
                              Description = i.Description
                          }).FirstOrDefaultAsync();
        }
    }
}