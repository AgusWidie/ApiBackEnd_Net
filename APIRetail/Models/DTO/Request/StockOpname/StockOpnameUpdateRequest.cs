namespace APIRetail.Models.DTO.Request
{
    public class StockOpnameUpdateRequest
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? ProductTypeId { get; set; }

        public long? ProductId { get; set; }

        public string Description { get; set; }

        public long? StockFirst { get; set; }

        public long? StockOpnameDefault { get; set; }

        public DateOnly? StockOpnameDate { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

    }
}
