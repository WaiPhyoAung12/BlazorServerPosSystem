using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblProduct
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string BarCode { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? UpdatedUserId { get; set; }

    public DateTime? UpdatedDateTime { get; set; }

    public bool DeleteFlag { get; set; }
}
