using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblSaleDetail
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public string? UpdatedUserId { get; set; }

    public DateTime? UpdatedDateTime { get; set; }
}
