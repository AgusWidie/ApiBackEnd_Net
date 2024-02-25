namespace APIRetail.Models.DTO.Request
{
    public class PurchaseOrderHeaderAddRequest
    {
        public long CompanyId { get; set; }

        public long BranchId { get; set; }

        public string PurchaseNo { get; set; }

        public DateTime PurchaseDate { get; set; }

        public long SupplierId { get; set; }

        public long? Quantity { get; set; }

        public long? Total { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public List<PurchaseOrderDetailAddRequest> PurchaseOrderDetailAddRequest { get; set; }
    }
}
