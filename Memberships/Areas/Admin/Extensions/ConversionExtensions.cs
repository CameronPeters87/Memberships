using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
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

        // For ProductItems ViewModel
        public static async Task<IEnumerable<ProductItemModel>> Convert
            (this IQueryable<ProductItem> productItems, ApplicationDbContext db)
        {
            // If products table data is empty, return empty table list
            if (productItems.Count().Equals(0))
                return new List<ProductItemModel>();

            // Link using linq
            return await (from pi in productItems
                          select new ProductItemModel
                          {
                              // We wont fill collections, because we dont need it in index view
                              ItemId = pi.ItemId,
                              ProductId = pi.ProductId,
                              ItemTitle = db.Items.FirstOrDefault(
                                  i => i.Id.Equals(pi.ItemId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(
                                  p => p.Id.Equals(pi.ProductId)).Title
                          }).ToListAsync();
        }

        // Take one ProductItem and Convert into one ProductItemModel
        public static async Task<ProductItemModel> Convert
            (this ProductItem productItem, ApplicationDbContext db)
        {
            // Link using linq
            var model = new ProductItemModel
            {
                ProductId = productItem.ProductId,
                ItemId = productItem.ItemId,
                Products = await db.Products.ToListAsync(),
                Items = await db.Items.ToListAsync(),
                ItemTitle = (await db.Items.FirstOrDefaultAsync(i =>
                   i.Id.Equals(productItem.ItemId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p =>
                   p.Id.Equals(productItem.ProductId))).Title
            };

            return model;
        }
    }
}