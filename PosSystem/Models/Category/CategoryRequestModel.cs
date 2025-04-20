using Microsoft.AspNetCore.Components.Forms;

namespace PosSystem.Models.Category;

public class CategoryRequestModel
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string ImageName { get; set; }
    public IBrowserFile? ImageFile { get; set; }
}
