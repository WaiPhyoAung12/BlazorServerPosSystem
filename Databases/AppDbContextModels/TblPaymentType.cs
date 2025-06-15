using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblPaymentType
{
    public int Id { get; set; }

    public string PaymentName { get; set; } = null!;

    public int PaymentMethod { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public DateTime? UpdatedDateTime { get; set; }

    public string? UpdatedUserId { get; set; }

    public bool DeleteFlag { get; set; }
}
