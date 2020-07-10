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
                                  orderby s.Title
                                  select new ProductSection
                                  {
                                      Id = s.Id,
                                      Title = s.Title,
                                      ItemTypeId = i.ItemTypeId
                                  }).ToListAsync();

            var result = sections.Distinct(new ProductSectionEqualityComparer()).ToList();

            var model = new ProductSectionModel
            {
                Sections = result,
                Title = await (from p in db.Products
                               where p.Id.Equals(productId)
                               select p.Title).FirstOrDefaultAsync()
            };

            return model;
        }
    }
}