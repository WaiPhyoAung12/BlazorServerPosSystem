using System.Security.Claims;

namespace PosSystem.Services.Shared;

public class HttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string UserId => GetUserId();

    private string? GetUserId()
    {
        var userId = GetClaimValue("UserId");
        return userId;
    }

    public string? GetClaimValue(string key)
    {
        var valueResult = _httpContextAccessor?
            .HttpContext?
            .User?
            .Claims?
            .FirstOrDefault(x => x.Type == key)?
            .Value;

        return valueResult;
    }
}
