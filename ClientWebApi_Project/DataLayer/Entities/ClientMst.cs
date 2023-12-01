using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class ClientMst
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Image { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Ctc { get; set; }

    public string? DocumentName { get; set; }

    public string? Document { get; set; }

    public string? Token { get; set; }

    public DateTime? TokenExpiryTime { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
