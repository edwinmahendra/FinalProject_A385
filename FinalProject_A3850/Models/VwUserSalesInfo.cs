using System;
using System.Collections.Generic;

namespace FinalProject_A3850.Models;

public partial class VwUserSalesInfo
{
    public Guid? UserId { get; set; }

    public string? Username { get; set; }

    public string? Status { get; set; }

    public DateTime? UserAddedDate { get; set; }

    public DateTime? UserUpdatedDate { get; set; }

    public Guid? SalesId { get; set; }

    public string? SalesName { get; set; }

    public int? JumlahPenjualan { get; set; }

    public decimal? Komisi { get; set; }

    public DateTime? SalesAddedDate { get; set; }

    public DateTime? SalesUpdatedDate { get; set; }
}
