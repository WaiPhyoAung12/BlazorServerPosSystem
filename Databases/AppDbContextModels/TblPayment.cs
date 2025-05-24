using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblPayment
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public int PaymentType { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal ChangeGiven { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? UpdatedDateTime { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public string? UpdatedUserId { get; set; }
}
