
using System.IO;

namespace PosSystem.Middlewares;

public class CookieMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var path = context.Request.Path;

        var ignorePathList = new[]
       {
            "/authentication/login",
            "/_framework/blazor.web.js",
            "/theme/assets/json/locales/fr.json"
        };

        if (ignorePathList.Any(x => x.Equals(path, StringComparison.OrdinalIgnoreCase)) ||
            path.ToString().StartsWith("/_blazor"))
        {
            await next.Invoke(context);
            return;
        }
        if (context.Request.Path.ToString().ToLower() == "/authentication/login")
        {
            await next.Invoke(context);
            return;
        }

        if (context.User.Identity is { IsAuthenticated: false })
        {
            context.Response.Redirect("/authentication/login");
            await next.Invoke(context);
            return;
        }

        await next.Invoke(context);
    }
}
