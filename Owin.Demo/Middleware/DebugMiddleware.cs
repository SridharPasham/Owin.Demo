using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFunc = System.Func<
System.Collections.Generic.IDictionary<string, object>,
System.Threading.Tasks.Task
>;

namespace Owin.Demo.Middleware
{
    public class DebugMiddleware
    {
        AppFunc _next;
        DebugMiddlewareOptions _options;

        public DebugMiddleware(AppFunc next, DebugMiddlewareOptions options)
        {
            _next = next;
            _options = options;

            if (_options.OnIncomingRequest == null)
            {
                _options.OnIncomingRequest = (ctnx) => { Debug.WriteLine("Incoming request: " + ctnx.Request.Path); };
            }

            if (_options.OnOutgoingRequest == null)
            {
                _options.OnOutgoingRequest = (ctnx) => { Debug.WriteLine("Outgoing request: " + ctnx.Request.Path); };
            }
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);

            _options.OnIncomingRequest(ctx);
            await _next(environment);
            _options.OnOutgoingRequest(ctx);
            
        }
    }
}