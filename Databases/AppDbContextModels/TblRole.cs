using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool DeleteFlag { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public bool? UpdatedUserId { get; set; }

    public DateTime? UpdatedDateTime { get; set; }
}
