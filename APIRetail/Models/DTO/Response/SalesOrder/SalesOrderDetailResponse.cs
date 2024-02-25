namespace APIRetail.Models.DTO.Response
{
    public class SalesOrderDetailResponse
    {
        public long? SalesHeaderId { get; set; }

        public long? ProductTypeId { get; set; }

        public string? ProductTypeName { get; set; }

        public long? ProductId { get; set; }

        public string? ProductName { get; set; }

        public int? Quantity { get; set; }

        public int? Price { get; set; }

        public long? Subtotal { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
