namespace APIRetail.Models.DTO.Request
{
    public class PurchaseOrderHeaderUpdateRequest
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public long BranchId { get; set; }

        public string PurchaseNo { get; set; }

        public DateTime PurchaseDate { get; set; }

        public long SupplierId { get; set; }

        public long? Quantity { get; set; }

        public long? Total { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public List<PurchaseOrderDetailAddRequest> PurchaseOrderDetailUpdateRequest { get; set; }
    }
}
