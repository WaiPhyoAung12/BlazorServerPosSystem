namespace PosSystem.Models.PaymentType;

public class PaymentTypeModel
{
    public int Id { get; set; }
    public string PaymentName { get; set; }
    public int PaymentMethod { get; set; }
}
public class PaymentTypeModelList
{
    public List<PaymentTypeModel> PaymentTypeModels { get; set; } = new();
}
