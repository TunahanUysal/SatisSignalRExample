namespace SatisSignalRExample.Subscription.Middleware
{
    public static class DatabaseSubscriptionMiddleware
    {
        public static void UseDatabaseSubscription<T>(this IApplicationBuilder builder, string tableName) where T : class, IDataBaseSubscription
        {
            var subscription = (T)builder.ApplicationServices.GetService(typeof(T));

            subscription.Configure(tableName);
        }
    }
}
