namespace APIRetail.Models.DTO.Request
{
    public class CompanyUpdateRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Telp { get; set; }

        public string Fax { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
