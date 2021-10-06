using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WazeCredit.Services.LifetimeExample;

namespace WazeCredit.Middleware
{
    public class CustomMiddleware
    {

        /// <summary>
        /// Represents the next request that is being called
        /// </summary>
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        /**
         * Differing from Controllers, Middlewares allows injection direct into InvokeAsync
         */

        public async Task InvokeAsync(HttpContext context, TransientService transientService, ScopedService scopedService, SingletonService singletonService)
        {
            context.Items.Add("CustomMiddlewareTransient", $"Transient Middleware - {transientService.GetGuid()}");
            context.Items.Add("CustomMiddlewareScoped", $"Scoped Middleware - {scopedService.GetGuid()}");
            context.Items.Add("CustomMiddlewareSingleton", $"Singleton Middleware - {singletonService.GetGuid()}");

            // Calling the delegate (or middleware) in the pipeline
            await this._next(context);
        }
    }
}
