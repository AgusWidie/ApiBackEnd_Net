namespace APIRetail.Models.DTO.Request
{
    public class PurchaseOrderRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long CompanyId { get; set; }

        public long BranchId { get; set; }

        public long? PurchaseHeaderId { get; set; }

        public DateTime? PurchaseDateFrom { get; set; }

        public DateTime? PurchaseDateTo { get; set; }
    }
}
