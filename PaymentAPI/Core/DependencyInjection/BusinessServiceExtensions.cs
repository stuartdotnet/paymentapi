using PaymentAPI.Business;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BusinessServiceExtensions
    {
        public static IServiceCollection AddBusinessServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
