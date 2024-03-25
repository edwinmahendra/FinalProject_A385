using System;
using System.Collections.Generic;

namespace FinalProject_A3850.Models;

public partial class MstSale
{
    public Guid IdSales { get; set; }

    public string? Name { get; set; }

    public int? JumlahPenjualan { get; set; }

    public decimal? Komisi { get; set; }

    public DateTime? DtAdded { get; set; }

    public DateTime? DtUpdated { get; set; }

    public Guid? IdUserAdded { get; set; }

    public Guid? IdUserUpdated { get; set; }

    public virtual ICollection<MstUser> MstUsers { get; set; } = new List<MstUser>();
}
