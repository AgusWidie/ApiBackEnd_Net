namespace APIRetail.Models.DTO.Request
{
    public class SalesOrderHeaderAddRequest
    {
        public long CompanyId { get; set; }

        public long BranchId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? SalesOrderDate { get; set; }

        public long? SalesId { get; set; }

        public long CustomerId { get; set; }

        public string? Description { get; set; }

        public int? Quantity { get; set; }

        public long? Total { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }


        public List<SalesOrderDetailAddRequest> SalesOrderDetailAddRequest { get; set; }
    }
}
