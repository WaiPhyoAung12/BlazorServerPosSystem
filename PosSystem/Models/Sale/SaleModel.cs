namespace PosSystem.Models.Sale;

public class SaleModel
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string UserId { get; set; }
}
