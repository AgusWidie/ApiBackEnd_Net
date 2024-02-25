namespace APIRetail.Models.DTO.Response
{
    public class PurchaseOrderResponse
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public long BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public string PurchaseNo { get; set; } = "";

        public DateTime? PurchaseDate { get; set; }

        public long SupplierId { get; set; }

        public string SupplierName { get; set; } = "";

        public long? Quantity { get; set; }

        public long? Total { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
