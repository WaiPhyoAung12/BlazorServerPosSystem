using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblSale
{
    public int Id { get; set; }

    public DateTime SaleDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string CreatedUserId { get; set; } = null!;
}
