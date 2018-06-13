using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GlobalWatchSystem
{
    public class CommonDataActionFilter : IActionFilter
    {
        public bool AllowMultiple
        {
            get { return false; } 
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            return null;
        }
    }
}