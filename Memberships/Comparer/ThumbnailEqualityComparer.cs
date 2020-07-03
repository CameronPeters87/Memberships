using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memberships.Comparer
{
    // Will compare the thumbnails for the products to make sure they not the same
    public class ThumbnailEqualityComparer : IEqualityComparer<ThumbnailModel>
    {
        public bool Equals(ThumbnailModel thumb1, ThumbnailModel thumb2)
        {
            // return true if ProductID in both thumbs are equal
            return thumb1.ProductId.Equals(thumb2.ProductId); 
        }

        public int GetHashCode(ThumbnailModel thumb)
        {
            return thumb.ProductId;
        }
    }
}