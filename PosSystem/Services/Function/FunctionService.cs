using PosSystem.Domain.Function;
using PosSystem.Models.Function;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Function;

public class FunctionService : IFunctionService
{
    private readonly FunctionRepo _functionRepo;

    public FunctionService(FunctionRepo functionRepo)
    {
        _functionRepo = functionRepo;
    }
    public async Task<Result<FunctionListResponseModel>> GetAllFunction()
    {
        var result=await _functionRepo.GetAllFunction();
        return result;
    }

    public async Task<Result<FunctionListResponseModel>> GetFunctionsByRoleId(int roleId)
    {
        var result=await _functionRepo.GetFunctionsByRoleId(roleId);
        return result;
    }
}
