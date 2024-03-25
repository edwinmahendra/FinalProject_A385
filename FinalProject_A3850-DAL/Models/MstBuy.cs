using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_A3850_DAL.Models
{
    public class MstBuy
    {
        public Guid Id { get; set; }
        public int Tenor { get; set; }
        public decimal DownPayment { get; set; }
        public decimal Tax { get; set; }
        public decimal Price { get; set; }
        public string? PaymentStatus { get; set; }
        public string? CustomerName { get; set; }
        public string? ModelName { get; set; }
        public string? BrandName { get; set; }
        public Guid CarId { get; set; }
        public Guid CustId { get; set; }
        public DateTime DtAdded { get; set; }
        public DateTime? DtUpdated { get; set; }
        public Guid? IdUserAdded { get; set; }
        public Guid? IdUserUpdated { get; set; }
    }
}
