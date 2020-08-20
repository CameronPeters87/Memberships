using Memberships.Entities;
using Memberships.Models;
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

        public static async Task Register(this IDbSet<UserSubscription> userSubscriptions,
            int subscriptionId, string userId)
        {
            var exists = await Task.Run(() => userSubscriptions.CountAsync(
                s => s.SubscriptionId.Equals(subscriptionId) &&
                s.UserId.Equals(userId))) > 0;

            if (!exists)
            {
                await Task.Run(() => userSubscriptions.Add(
                    new UserSubscription
                    {
                        UserId = userId,
                        SubscriptionId = subscriptionId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.MaxValue
                    }));
            }
        }

        public static async Task<bool> RegisterUserSubscriptionCode(
            string userId, string code)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var subscriptionId = await db.Subscriptions.GetSubscriptionIdByRegistrationCode(code);

                if (subscriptionId < 0)
                    return false;

                await db.UserSubscriptions.Register(subscriptionId, userId);

                // Detect if database has changed
                if (db.ChangeTracker.HasChanges())
                    await db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}