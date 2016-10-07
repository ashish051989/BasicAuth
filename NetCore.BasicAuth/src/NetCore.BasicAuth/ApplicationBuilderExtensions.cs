using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.BasicAuth
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder appBuilder)
        {
            var options = appBuilder.ApplicationServices.GetService<IOptions<BasicOptions>>();
            return appBuilder.UseMiddleware<BasicAuthMiddleware>(options);
        }
    }
}
