namespace APIRetail.Models.DTO.Request
{
    public class PurchaseOrderDetailAddRequest
    {
        public long? PurchaseHeaderId { get; set; }

        public long? ProductTypeId { get; set; }

        public long? ProductId { get; set; }

        public int? Quantity { get; set; }

        public int? Price { get; set; }

        public long? Subtotal { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
