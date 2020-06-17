using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Minitab.Assignment.Api.Middleware;
using Minitab.Assignment.Api.Models;
using Minitab.Assignment.Common.Models;
using Minitab.Assignment.CrmStub;
using Minitab.Assignment.CrmStub.Interfaces;
using Minitab.Assignment.DataContext;
using Minitab.Assignment.Services;
using Minitab.Assignment.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;

namespace Minitab.Assignment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MinitabDbContext>(options =>
                 options.UseSqlite("Data Source = Minitab.db"));

            // Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minitab Address Validation",
                    Version = "v1",
                    Description = "Sample API By Frank Malinowski"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddHttpClient<IAddressService, AddressService>()
                .AddPolicyHandler(GetRetryPolicy());

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IAddressValidationService, UspsAddressValidationService>();
            services.AddTransient<ICrmRepository, CrmRepository>();

            services.Configure<UspsSettings>(Configuration.GetSection("UspsSettings"));
            services.AddOptions();

            services
               .AddControllers()
                   .ConfigureApiBehaviorOptions(options =>
                   {
                       options.InvalidModelStateResponseFactory = context =>
                       {
                           var error = new ErrorMessages
                           {
                               Message = "The request is invalid.",
                               Errors = context.ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToList()
                           };

                           var result = new BadRequestObjectResult(error);
                           result.ContentTypes.Add(MediaTypeNames.Application.Json);
                           result.ContentTypes.Add(MediaTypeNames.Application.Xml);

                           return result;
                       };
                   })
               .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            //Global error handling
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minitab");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Retry Policy
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
