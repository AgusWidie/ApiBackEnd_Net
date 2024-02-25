namespace APIRetail.Models.DTO.Request
{
    public class DailyStockUpdateRequest
    {
        public long? Id { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? ProductTypeId { get; set; }

        public long? ProductId { get; set; }

        public long? StockFirst { get; set; }

        public long? StockBuy { get; set; }

        public long? StockBuyPrice { get; set; }

        public long? StockSell { get; set; }

        public long? StockSellPrice { get; set; }

        public long? StockLast { get; set; }

        public DateOnly StockDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
