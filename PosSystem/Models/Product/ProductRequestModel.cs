using Microsoft.AspNetCore.Components.Forms;
using PosSystem.Models.PageSetting;

namespace PosSystem.Models.Product;

public class ProductRequestModel
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string BarCode { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public string ImagePath { get; set; } = null!;

    public string ImageName { get; set; }
    public string OldImageName { get; set; }
    public IBrowserFile ImageFile { get; set; }

}
public class ProductListRequestModel
{
    public int? CategoryId { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
