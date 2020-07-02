using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class UserSubscriptionViewModel
    {
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<UserSubscriptionModel> UserSubscription { get; set; }
        public bool DisableDropDownList { get; set; }
        public string UserId { get; set; }
    }
}