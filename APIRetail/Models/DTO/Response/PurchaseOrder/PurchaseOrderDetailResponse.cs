namespace APIRetail.Models.DTO.Response
{
    public class PurchaseOrderDetailResponse
    {
        public long? PurchaseHeaderId { get; set; }

        public long? ProductTypeId { get; set; }

        public string ProductTypeName { get; set; } = "";

        public long? ProductId { get; set; }

        public string? ProductName { get; set; } = "";

        public int? Quantity { get; set; }

        public int? Price { get; set; }

        public long? Subtotal { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }
    }
}
