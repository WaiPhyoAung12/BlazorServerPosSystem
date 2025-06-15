using Databases.AppDbContextModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PosSystem.Models.Function;
using PosSystem.Models.Shared;

namespace PosSystem.Domain.Function;

public class FunctionRepo
{
    private readonly AppDbContext _appDbContext;

    public FunctionRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<Result<FunctionListResponseModel>> GetAllFunction()
    {
        var result = await _appDbContext.TblFunctions.Where(x => x.DeleteFlag == false).ToListAsync();
        if (result.Count <= 0)
            return Result<FunctionListResponseModel>.Fail("Data not found");

        FunctionListResponseModel functionListResponseModel = result.Adapt<FunctionListResponseModel>();
        return Result<FunctionListResponseModel>.Success("Success", functionListResponseModel);
    }

    public async Task<Result<FunctionListResponseModel>> GetFunctionsByRoleId(int roleId)
    {

        var result = (from rf in _appDbContext.TblRoleFunctions
                     join f in _appDbContext.TblFunctions
                     on rf.FunctionId equals f.Id
                     where rf.DeleteFlag == false
                     && rf.RoleId == roleId
                     && f.DeleteFlag == false
                     select f).GroupBy(x => x.Module)
             .Select(g => g.First())
             .ToList();

        if (result.Count <= 0)
            return Result<FunctionListResponseModel>.Fail("Data not found");

        var functionList=result.Adapt<List<FunctionResponseModel>>();
        FunctionListResponseModel functionListResponseModel = new();
        functionListResponseModel.FunctonListModel = functionList;  
        return Result<FunctionListResponseModel>.Success("Success", functionListResponseModel);
    }

}
