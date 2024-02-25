namespace APIRetail.Models.DTO.Request
{
    public class DailyStockAddRequest
    {
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

        public string CreateBy { get; set; }

        public DateTime? CreteDate { get; set; }
    }
}
