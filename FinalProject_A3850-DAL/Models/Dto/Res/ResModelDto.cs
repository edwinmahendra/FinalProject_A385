using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_A3850_DAL.Models.Dto.Res
{
    public class ResModelsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public DateTime DtAdded { get; set; }
        public DateTime DtUpdated { get; set; }
    }
}