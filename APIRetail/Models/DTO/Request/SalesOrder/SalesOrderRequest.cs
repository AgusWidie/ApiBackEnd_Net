namespace APIRetail.Models.DTO.Request
{
    public class SalesOrderRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long CompanyId { get; set; }

        public long BranchId { get; set; }

        public long? SalesHeaderId { get; set; }

        public DateTime? SalesOrderDateFrom { get; set; }

        public DateTime? SalesOrderDateTo { get; set; }

    }
}
