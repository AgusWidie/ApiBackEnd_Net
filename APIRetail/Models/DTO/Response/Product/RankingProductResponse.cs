namespace APIRetail.Models.DTO.Response
{
    public class RankingProductResponse
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string? CompanyName { get; set; }

        public long? BranchId { get; set; }

        public string? BranchName { get; set; }

        public long? ProductTypeId { get; set; }

        public string? ProductTypeName { get; set; }

        public long? ProductId { get; set; }

        public string? ProductName { get; set; }

        public long? StockBuy { get; set; }

        public long? StockSell { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}
