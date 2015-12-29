using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Owin.Demo.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<Microsoft.Owin.Security.AuthenticationDescription> AuthProviders { get; set; }
    }
}