namespace APIRetail.Models.DTO.Response
{
    public class CustomerResponse
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public string Name { get; set; } = "";

        public string Address { get; set; } = "";

        public string Telephone { get; set; } = "";

        public string WhatsApp { get; set; } = "";

        public string Email { get; set; } = "";

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
