using System;
using System.Collections.Generic;

namespace FinalProject_A3850_DAL.Models;

public partial class MstType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? DtAdded { get; set; }

    public DateTime? DtUpdated { get; set; }

    public Guid? IdUserAdded { get; set; }

    public Guid? IdUserUpdated { get; set; }

    public virtual ICollection<MstCar> MstCars { get; set; } = new List<MstCar>();
}
