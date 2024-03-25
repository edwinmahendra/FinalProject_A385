using System;
using System.Collections.Generic;

namespace FinalProject_A3850_DAL.Models;

public partial class MstUser
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Status { get; set; }

    public Guid? IdSales { get; set; }

    public DateTime? DtAdded { get; set; }

    public DateTime? DtUpdated { get; set; }

    public Guid? IdUserAdded { get; set; }

    public Guid? IdUserUpdated { get; set; }

    public virtual MstSale? IdSalesNavigation { get; set; }
}
