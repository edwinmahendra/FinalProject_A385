using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_A3850_DAL.Models.Dto.Req
{
    public class ReqCarDto
    {
        public Guid Id { get; set; }

        public int? EngineSize { get; set; }

        public string? FuelType { get; set; }

        public int? ManufactureYear { get; set; }

        public string? CdChassis { get; set; }

        public string? CdEngine { get; set; }

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public int? UsageId { get; set; }

        public int? ModelId { get; set; }

        public DateTime? DtAdded { get; set; }

        public DateTime? DtUpdated { get; set; }

        public Guid? IdUserAdded { get; set; }

        public Guid? IdUserUpdated { get; set; }

        public virtual MstBrand? Brand { get; set; }

        public virtual MstModel? Model { get; set; }

        public virtual ICollection<TrnCarPurchase> TrnCarPurchases { get; set; } = new List<TrnCarPurchase>();

        public virtual MstType? Type { get; set; }

        public virtual MstUsage? Usage { get; set; }
    }
}
