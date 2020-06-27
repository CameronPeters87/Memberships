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
            // If products table data is empty, return empty table list
            if (products.Count().Equals(0))
                return new List<ProductModel>();

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

        // Convert List ProductModel into 1 ProductModel
        // Copy Above class and remove IEnumerable
        public static async Task<ProductModel> Convert
            (this Product products, ApplicationDbContext db)
        {
            // We need the lists thats in the db
            var texts = await db.ProductLinkTexts.FirstOrDefaultAsync(
                p => p.Id.Equals(products.ProductLinkTextId));
            var types = await db.ProductTypes.FirstOrDefaultAsync(
                p => p.Id.Equals(products.ProductTypeId));

            // Link using linq
                   var model = new ProductModel
                   {
                       Id = products.Id,
                       Title = products.Title,
                       Description = products.Description,
                       ImageUrl = products.ImageUrl,
                       ProductLinkTextId = products.ProductLinkTextId,
                       ProductTypeId = products.ProductTypeId,
                       ProductLinkTexts = new List<ProductLinkText>(),
                       ProductTypes = new List<ProductType>()
                   };

            // Add data to model
            model.ProductLinkTexts.Add(texts);
            model.ProductTypes.Add(types);

            return model;
        }
    }
}