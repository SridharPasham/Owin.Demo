using Owin.Demo.Middleware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Nancy.Owin;
using Nancy;
using System.Web.Http;
using Microsoft.Owin.Security.Cookies;

namespace Owin.Demo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseDegubMiddleware(new DebugMiddlewareOptions()
            {
                OnIncomingRequest = (ctx) =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    ctx.Environment["DebugStartWatch"] = watch;
                },
                OnOutgoingRequest = (ctx) =>
                {
                    var watch = (Stopwatch)ctx.Environment["DebugStartWatch"];
                    watch.Stop();
                    Debug.WriteLine("Request took: " + watch.ElapsedMilliseconds + " ms");

                }
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "AppliationCookie",
                LoginPath = new Microsoft.Owin.PathString("/Auth/Login")
            });

            app.UseFacebookAuthentication(new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
            {
                AppId = "[INSERT APP ID FACEBOOK APP]",
                AppSecret = "[INSERT APP SECRET FOR FACEBOOK APP]",
                SignInAsAuthenticationType = "ApplicationCookie"
            });

            app.UseTwitterAuthentication(new Microsoft.Owin.Security.Twitter.TwitterAuthenticationOptions
            {
                ConsumerKey = "[INSERT CONSUMER KEY FOR TWITTER APP]",
                ConsumerSecret = "[INSERT CONSUMER SECRET FOR TWITTER APP]",
                SignInAsAuthenticationType = "ApplicationCookie",
                BackchannelCertificateValidator = null
            });

            // how to use katana authentication in middle ware
            app.Use(async (ctx, next) => {
                
                if(ctx.Authentication.User.Identity.IsAuthenticated)
                {
                    Debug.WriteLine("User authenticated, User Name: " + ctx.Authentication.User.Identity.Name);
                }
                else
                {
                    Debug.WriteLine("User not authenticated.");
                }
                await next();
            
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

            app.Map("/nancy", mappedApp => { mappedApp.UseNancy(); });

            // this makes authentication fail. So add above line
            //app.UseNancy(conf => {

            //    conf.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            
            //});

            // MVC 5 wont work in OWIN but works along with Katana(Microsoft owin implementation)
            // comment this because MVC 5 Wont work if any Katana middle ware serves the request.
            //app.Use(async (ctx, next) =>
            //{
            //    await ctx.Response.WriteAsync("Hello world");

            //});
        }
    }
}