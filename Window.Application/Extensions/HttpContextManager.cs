using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Window.Application.Extensions
{
    public static class HttpContextManager
    {
        public static string GetUserIP(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        public static string GetUrlReferer(this HttpRequest request)
        {
            return request.Headers["Referer"].ToString();
        }
    }
}
