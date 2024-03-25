using System;
using System.Collections.Generic;

namespace FinalProject_A3850.Models;

public partial class MstModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int BrandId { get; set; }

    public DateTime? DtAdded { get; set; }

    public DateTime? DtUpdated { get; set; }

    public Guid? IdUserAdded { get; set; }

    public Guid? IdUserUpdated { get; set; }

    public virtual MstBrand Brand { get; set; } = null!;

    public virtual ICollection<MstCar> MstCars { get; set; } = new List<MstCar>();
}
