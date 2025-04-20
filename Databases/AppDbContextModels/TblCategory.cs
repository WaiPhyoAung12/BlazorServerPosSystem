using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public string? CreatedUserId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string? UpdatedUserId { get; set; }

    public DateTime? UpdatedDateTime { get; set; }

    public bool DeleteFlag { get; set; }
}
