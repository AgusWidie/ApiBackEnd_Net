namespace APIRetail.Models.DTO.Response
{
    public class DailyStockResponse
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public long? ProductTypeId { get; set; }

        public string ProductTypeName { get; set; } = "";

        public long? ProductId { get; set; }

        public string ProductName { get; set; } = "";

        public long? StockFirst { get; set; }

        public long? StockBuy { get; set; }

        public long? StockBuyPrice { get; set; }

        public long? StockSell { get; set; }

        public long? StockSellPrice { get; set; }

        public long? StockLast { get; set; }

        public DateOnly StockDate { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
