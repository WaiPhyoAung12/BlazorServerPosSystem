using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblRoleFunction
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int FunctionId { get; set; }

    public bool DeleteFlag { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? UpdatedUserId { get; set; }

    public DateTime? UpdatedDateTime { get; set; }
}
