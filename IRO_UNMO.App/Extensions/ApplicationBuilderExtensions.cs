using IRO_UNMO.App.Subscription;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace IRO_UNMO.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSqlTableDependency<T>(this IApplicationBuilder services, string connectionString)
    where T : IDatabaseSubscription
        {
            var serviceProvider = services.ApplicationServices;
            var subscription = serviceProvider.GetService<T>();
            subscription.Configure(connectionString);
        }
    }
}
