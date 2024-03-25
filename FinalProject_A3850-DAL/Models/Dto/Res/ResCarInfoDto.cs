using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_A3850_DAL.Models.Dto.Res
{
    public class ResCarInfoDto
    {
        public Guid Id { get; set; }
        public int EngineSize { get; set; }
        public string? FuelType { get; set; }
        public int ManufactureYear { get; set; }
        public string? CdChassis { get; set; }
        public string? CdEngine { get; set; }
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public string? TypeName { get; set; }
        public string? UsageName { get; set; }
        public DateTime DtAdded { get; set; }
        public DateTime DtUpdated { get; set; }
        public Guid IdUserAdded { get; set; }
        public Guid IdUserUpdated { get; set; }
    }
}
