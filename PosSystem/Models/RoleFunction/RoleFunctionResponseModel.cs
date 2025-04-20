namespace PosSystem.Models.RoleFunction
{
    public class RoleFunctionResponseModel
    {
        public int Id { get; set; }
        public int FunctionId { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public string ModuleName { get; set; }
    }
}
