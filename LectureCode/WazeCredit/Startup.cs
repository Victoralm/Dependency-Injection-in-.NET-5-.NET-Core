using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Data;
using WazeCredit.Middleware;
using WazeCredit.Models;
using WazeCredit.Services;
using WazeCredit.Services.LifetimeExample;
using WazeCredit.Utility.AppSettingsClasses;
using WazeCredit.Utility.DI_Config;

namespace WazeCredit
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            /**
             * Three types of lifetime services:
             * Transient<'interface', 'desired implementation to be used'>
             * Code
             * Singleton
             */
            services.AddTransient<IMarketForecaster, MarketForecaster>();
            //services.AddTransient<IMarketForecaster, MarketForecasterV2>();

            // Injects the dependency only if another implementation of the same interface doesn't exist on the application stack
            // Useful on complex applications to avoid injection of multiple implementation of the same interface
            services.TryAddTransient<IMarketForecaster, MarketForecasterV2>();

            // Replacing a registered injected service on the application stack
            services.AddTransient<IMarketForecaster, MarketForecasterV2>();
            services.Replace(ServiceDescriptor.Transient<IMarketForecaster, MarketForecaster>());

            // Removing all the implementations of some injected service on the application stack
            services.RemoveAll<IMarketForecaster>();
            // Adding the correct one again
            services.AddTransient<IMarketForecaster, MarketForecaster>();

            //services.AddScoped<IValidationChecker, AddressValidationChecker>();
            //services.AddScoped<IValidationChecker, CreditValidationChecker>();
            // To avoid Duplications: Prevent the same injection to happen more than once on the application stack
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>());
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());  // Only the first one will be injected
            // Even better:
            services.TryAddEnumerable(new []
            {
                ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>(),
            });
            services.AddScoped<ICreditValidator, CreditValidator>();


            // Configurations => gets the sections of appsettings.json and associate with the classes
            /*services.Configure<WazeForecastSettings>(Configuration.GetSection("WazeForecast"));
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));*/
            services.AddAppSettingsConfig(Configuration);

            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();

            #region Conditional Implementation
            services.AddScoped<CreditApprovedHigh>();
            services.AddScoped<CreditApprovedLow>();
            services.AddScoped<Func<CreditApprovedEnum, ICreditApproved>>(ServiceProvider => range =>
            {
                switch (range)
                {
                    case CreditApprovedEnum.Low:
                        return ServiceProvider.GetService<CreditApprovedLow>();
                    case CreditApprovedEnum.High:
                        return ServiceProvider.GetService<CreditApprovedHigh>();
                    default:
                        return ServiceProvider.GetService<CreditApprovedLow>();
                }
            });
            #endregion

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            loggerFactory.AddFile("logs/creditApp-log-{Date}.txt");

            app.UseAuthentication();
            app.UseAuthorization();

            // Registreing the Middleware (runs every tima a request is made)
            app.UseMiddleware<CustomMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
