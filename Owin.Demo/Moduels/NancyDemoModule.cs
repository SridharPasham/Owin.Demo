using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Owin;
using Nancy.Security;

namespace Owin.Demo.Moduels
{
    public class NancyDemoModule: NancyModule
    {
        public NancyDemoModule()
        {
            this.RequiresMSOwinAuthentication();

            Get["/Nancy"] = x =>
            {
                var env = Context.GetOwinEnvironment();

                var user = Context.GetMSOwinUser();

                return "Hello from Nancy! Your requested path " + env["owin.RequestPath"] + "<br/><br/> User Name: " + user.Identity.Name;
            };
        }
    }
}