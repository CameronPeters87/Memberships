using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memberships.Extensions
{
    // For ToSelectListItem
    public static class ReflectionExtensions
    {
        // Extension method
        // Returns a string of a property that we passed through in the parameter
        // We work with obj Type T
        // Generic type T
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            return item.GetType()
                .GetProperty(propertyName)
                .GetValue(item, null).ToString();
        }
    }
}