namespace FinalProject_A3850_DAL.Models.Dto.Req
{
    public class ReqModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public DateTime DtAdded { get; set; }
        public DateTime DtUpdated { get; set; }
    }
}