using Memberships.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Memberships.Extensions
{
    public static class SubscriptionExtension
    {
        public static async Task<int> GetSubscriptionIdByRegistrationCode(
            this IDbSet<Subscription> subscriptions, string code)
        {
            try
            {
                if (subscriptions == null || code == null || code.Length < 0)
                    return Int32.MinValue;

                var subscriptionId = await (
                    from s in subscriptions
                    where s.RegistrationCode == code
                    select s.Id).FirstOrDefaultAsync();

                return subscriptionId;
            }
            catch
            {
                return Int32.MinValue;
            }
        }
    }
}