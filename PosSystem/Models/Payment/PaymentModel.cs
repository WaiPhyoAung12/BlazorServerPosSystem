namespace PosSystem.Models.Payment;

public class PaymentModel
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int PaymentType { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeGiven { get; set; }
}
