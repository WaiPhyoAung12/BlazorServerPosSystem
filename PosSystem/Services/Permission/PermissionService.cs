using PosSystem.Services.RoleFunction;
using System.Security.Claims;

namespace PosSystem.Services.Permission;

public class PermissionService
{
    private readonly IRoleFunctionService _roleFunctionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionService(IRoleFunctionService roleFunctionService,IHttpContextAccessor httpContextAccessor)
    {
        _roleFunctionService = roleFunctionService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> CheckUserPermission(string functionCode)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
            return false;

        var roleIdClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        if (!int.TryParse(roleIdClaim, out int roleId))
            return false;

        var response = await _roleFunctionService.GetRoleFunctionsByRoleId(roleId);
        if (response.IsError)
            return false;

        bool result=response.Data!.Any(x=>x.FunctionCode==functionCode);
        return result;
    }
}
