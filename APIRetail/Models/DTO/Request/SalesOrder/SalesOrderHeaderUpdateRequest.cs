namespace APIRetail.Models.DTO.Request
{
    public class SalesOrderHeaderUpdateRequest
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public long BranchId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime SalesOrderDate { get; set; }

        public long? SalesId { get; set; }

        public long CustomerId { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        public long? Total { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public List<SalesOrderDetailAddRequest> SalesOrderDetailUpdateRequest { get; set; }
    }
}
