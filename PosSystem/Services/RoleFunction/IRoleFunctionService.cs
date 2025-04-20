using PosSystem.Models.RoleFunction;
using PosSystem.Models.Shared;

namespace PosSystem.Services.RoleFunction
{
    public interface IRoleFunctionService
    {
        Task<Result<List<RoleFunctionResponseModel>>>GetRoleFunctionsByRoleId(int roleId);    
    }
}
