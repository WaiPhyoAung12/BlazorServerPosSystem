namespace PosSystem.Models.Function;

public class FunctionResponseModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Module { get; set; }


}
public class FunctionListResponseModel
{
    public List<FunctionResponseModel> FunctonListModel { get; set; } = new();
}
