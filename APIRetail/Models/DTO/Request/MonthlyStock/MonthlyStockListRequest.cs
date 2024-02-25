namespace APIRetail.Models.DTO.Request
{
    public class MonthlyStockListRequest
    {

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? ProductTypeId { get; set; }

        public long? ProductId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}
