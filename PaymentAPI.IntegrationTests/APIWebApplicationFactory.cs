using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentAPI.Data;
using System.Linq;

namespace PaymentAPI.IntegrationTests
{
    public class APIWebApplicationFactory<StartUp>
        : WebApplicationFactory<StartUp> where StartUp : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<PaymentsContext>));

                services.Remove(descriptor);

                services.AddDbContext<PaymentsContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<PaymentsContext>();

                    db.Database.EnsureCreated();

                    Seeding.InitializeDbForTests(db);
                }
            });
        }
    }
}
