namespace APIRetail.Models.DTO.Request
{
    public class DailyStockListRequest
    {

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? ProductTypeId { get; set; }

        public long? ProductId { get; set; }

    }
}
