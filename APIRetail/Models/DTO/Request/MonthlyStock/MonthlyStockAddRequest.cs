namespace APIRetail.Models.DTO.Request
{
    public class MonthlyStockAddRequest
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

        public int? Year { get; set; }

        public int? Month { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
