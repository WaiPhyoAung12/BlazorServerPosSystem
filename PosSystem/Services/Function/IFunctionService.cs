using PosSystem.Models.Function;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Function;

public interface IFunctionService
{
    Task<Result<FunctionListResponseModel>> GetAllFunction();
    Task<Result<FunctionListResponseModel>> GetFunctionsByRoleId(int roleId);
}
