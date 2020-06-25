using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Title { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }
        [MaxLength(1024)]
        [DisplayName("Image Url: ")]
        public string ImageUrl { get; set; }
        public int ProductLinkTextId { get; set; }
        public int ProductTypeId { get; set; }
        public ICollection<ProductType> ProductTypes { get; set; }
        public ICollection<ProductLinkText> ProductLinkTexts { get; set; }
        
        // Return product type title
        public string ProductType { 
            get
            {
                // Return string title
                // If theres no product type, String is empty
                // Else if ProdType.Id == ProductTypeId in this model, display the title
                return ProductTypes == null || ProductTypes.Count.Equals(0) ? String.Empty : 
                    ProductTypes.First(pt => pt.Id.Equals(ProductTypeId)).Title;
            }
        }

        // Return product Link text title
        public string ProductLinkText
        {
            get
            {
                // Return string title
                // If theres no product type, String is empty
                // Else if ProdType.Id == ProductTypeId in this model, display the title
                return ProductLinkTexts == null || ProductLinkTexts.Count.Equals(0) ? String.Empty :
                    ProductLinkTexts.First(pt => pt.Id.Equals(ProductLinkTextId)).Title;
            }
        }
    }
}