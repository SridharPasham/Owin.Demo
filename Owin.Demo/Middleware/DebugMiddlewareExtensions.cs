using Owin.Demo.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Owin
{
    public static class DebugMiddlewareExtensions
    {
        public static void UseDegubMiddleware(this IAppBuilder app, DebugMiddlewareOptions options = null)
        {
            if (options == null)
            {
                options = new DebugMiddlewareOptions();
            }

            app.Use<DebugMiddleware>(options);
        }
    }
}