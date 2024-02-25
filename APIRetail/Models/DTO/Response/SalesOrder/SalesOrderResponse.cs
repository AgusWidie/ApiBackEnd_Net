namespace APIRetail.Models.DTO.Response
{
    public class SalesOrderResponse
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public long BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public string InvoiceNo { get; set; } = "";

        public DateTime? SalesOrderDate { get; set; }

        public long? SalesId { get; set; }

        public string SalesName { get; set; } = "";

        public long CustomerId { get; set; }

        public string CustomerName { get; set; } = "";

        public string Description { get; set; } = "";

        public int? Quantity { get; set; }

        public long? Total { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
