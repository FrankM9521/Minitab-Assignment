using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Minitab.Assignment.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Minitab.Assignment.Api.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected MinitabDbContext DbContext { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<MinitabDbContext>));
            
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<MinitabDbContext>(options =>
                    options.UseSqlite("Data Source = MtbAssignmentCustomers.db"));

                // Build the service provider.
                var sp = services.BuildServiceProvider();
              
                // Create a scope to obtain a reference to the database
               // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<MinitabDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    dbContext.Database.EnsureCreated();
                }
            });
        }
    }
}
