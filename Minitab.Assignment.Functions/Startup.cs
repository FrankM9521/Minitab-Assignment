using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Minitab.Assignment.Common.Models;
using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Services;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(Minitab.Assignment.Functions.Startup))]
namespace Minitab.Assignment.Functions
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<IAddressService, AddressService>(client =>
                client.BaseAddress = new Uri(UspsSettings.Instance.Url)
            )
            .AddPolicyHandler(GetRetryPolicy());

            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<IAddressService, AddressService>();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
