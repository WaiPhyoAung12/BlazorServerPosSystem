using PosSystem.Domain.RoleFunction;
using PosSystem.Models.RoleFunction;
using PosSystem.Models.Shared;

namespace PosSystem.Services.RoleFunction
{
    public class RoleFunctionService : IRoleFunctionService
    {
        private readonly RoleFunctionRepo _roleFunctionRepo;

        public RoleFunctionService(RoleFunctionRepo roleFunctionRepo)
        {
            _roleFunctionRepo = roleFunctionRepo;
        }
        
        public async Task<Result<List<RoleFunctionResponseModel>>> GetRoleFunctionsByRoleId(int roleId)
        {
            var result = await _roleFunctionRepo.GetRoleFunctionsByRoleId(roleId);
            return result;
        }
    }
}
