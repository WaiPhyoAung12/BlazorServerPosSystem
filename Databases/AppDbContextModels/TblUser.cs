using System;
using System.Collections.Generic;

namespace Databases.AppDbContextModels;

public partial class TblUser
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string Salt { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool DeleteFlag { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public string? UpdatedUserId { get; set; }
}
