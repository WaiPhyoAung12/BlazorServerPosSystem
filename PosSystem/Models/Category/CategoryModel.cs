using Microsoft.AspNetCore.Components.Forms;

namespace PosSystem.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? CreatedUserId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string? UpdatedUserId { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
        public string ImageName { get; set; }
    }
}
