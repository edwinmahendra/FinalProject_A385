using System;
using System.Collections.Generic;

namespace FinalProject_A3850.Models;

public partial class MstBrand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public DateTime? DtAdded { get; set; }

    public DateTime? DtUpdated { get; set; }

    public Guid? IdUserAdded { get; set; }

    public Guid? IdUserUpdated { get; set; }

    public virtual ICollection<MstCar> MstCars { get; set; } = new List<MstCar>();

    public virtual ICollection<MstModel> MstModels { get; set; } = new List<MstModel>();
}
