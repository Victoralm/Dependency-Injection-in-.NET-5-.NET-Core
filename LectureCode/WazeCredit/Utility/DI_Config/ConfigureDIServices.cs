using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using WazeCredit.Models;
using WazeCredit.Services;
using WazeCredit.Services.LifetimeExample;

namespace WazeCredit.Utility.DI_Config
{
    /// <summary>
    /// Custom class made to contains all the service injections needed for the application
    /// </summary>
    public static class ConfigureDIServices
    {
        /// <summary>
        /// Custom method to contain all the service injections needed for the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
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
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>(),
            });
            services.AddScoped<ICreditValidator, CreditValidator>();

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

            return services;
        }
    }
}
