using PaymentAPI.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataServiceExtensions
    {
        public static IServiceCollection AddDataServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IPaymentData, SqlPaymentData>();
            services.AddScoped<IAccountData, SqlAccountData>();

            return services;
        }
    }
}
