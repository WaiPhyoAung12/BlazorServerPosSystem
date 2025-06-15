using Databases.AppDbContextModels;
using PosSystem.Models.RoleFunction;
using PosSystem.Models.Shared;

namespace PosSystem.Domain.RoleFunction;

public class RoleFunctionRepo
{
    private readonly AppDbContext _appDbContext;

    public RoleFunctionRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<Result<List<RoleFunctionResponseModel>>>GetRoleFunctionsByRoleId(int roleId)
    {
        var response = from function in _appDbContext.TblFunctions
                       where function.DeleteFlag == false
                       join roleFunction in _appDbContext.TblRoleFunctions
                       on function.Id equals roleFunction.FunctionId
                       where roleFunction.DeleteFlag == false && roleFunction.RoleId == roleId
                       select new RoleFunctionResponseModel()
                       {
                           Id=roleFunction.Id,
                           FunctionId=roleFunction.FunctionId,
                           FunctionCode=function.Code,
                           FunctionName=function.Name,
                           ModuleName=function.Module,
                       };

        var roleFunctionList = response.ToList();
        if (roleFunctionList.Count <= 0)
            return Result<List<RoleFunctionResponseModel>>.Fail("Data not found");

        return Result<List<RoleFunctionResponseModel>>.Success("Success",roleFunctionList);

    }

}
