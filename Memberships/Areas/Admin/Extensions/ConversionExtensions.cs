using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Memberships.Areas.Admin.Extensions
{
    public static class ConversionExtensions
    {
        // Converts IEnum<Products> into IEnum<ProductModel>
        public static async Task<IEnumerable<ProductModel>> Convert
            (this IEnumerable<Product> products, ApplicationDbContext db)
        {
            // We need the lists thats in the db
            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            // Link using linq
            return from p in products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductTypeId = p.ProductTypeId,
                       ProductLinkTexts = texts,
                       ProductTypes = types
                   };
        }
    }
}