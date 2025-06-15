namespace PosSystem.Models.Function;

public class FunctionModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Module { get; set; }

    public bool DeleteFlag { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
