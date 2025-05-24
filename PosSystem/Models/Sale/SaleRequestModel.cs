using PosSystem.Models.Product;
using PosSystem.Models.SaleDetails;

namespace PosSystem.Models.Sale;

public class SaleRequestModel
{
    public decimal TotalAmount { get; set; }
    public decimal TotalCount { get; set; }
    public long OrderId { get; set; }
    public int PaymentType { get; set; }
    public decimal ComercialTax { get; set; }
    public decimal Changes { get; set; }

    public decimal AmountPaid { get; set; }
    public List<SaleDetailsModel> SaleDetailsList { get; set; } = new();
}
