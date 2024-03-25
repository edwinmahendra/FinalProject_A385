using System;
using System.Collections.Generic;

namespace FinalProject_A3850.Models;

public partial class TrnCarPurchase
{
    public Guid Id { get; set; }

    public int? Tenor { get; set; }

    public decimal? DownPayment { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Price { get; set; }

    public string? PaymentStatus { get; set; }

    public Guid? CarId { get; set; }

    public Guid? CustId { get; set; }

    public DateTime? DtAdded { get; set; }

    public DateTime? DtUpdated { get; set; }

    public Guid? IdUserAdded { get; set; }

    public Guid? IdUserUpdated { get; set; }

    public virtual MstCar? Car { get; set; }

    public virtual MstCustomer? Cust { get; set; }
}
