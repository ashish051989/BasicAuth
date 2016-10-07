using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.BasicAuth
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BasicOptions _options;

        public BasicAuthMiddleware(RequestDelegate next, IOptions<BasicOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                if (username == _options.Credentials.UserName && password == _options.Credentials.Password)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //Unauthorized
                }
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Request contains no Authorization header");//Unauthorized
            }
        }
    }
}
