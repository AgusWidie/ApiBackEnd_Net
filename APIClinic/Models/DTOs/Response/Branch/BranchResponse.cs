namespace APIClinic.Models.DTOs.Response
{
    public class BranchResponse
    {
        public long Id { get; set; }

        public long? ClinicId { get; set; }

        public string ClinicName { get; set; } = "";

        public string Name { get; set; } = "";

        public string Address { get; set; } = "";

        public string Telp { get; set; } = "";

        public string Fax { get; set; } = "";

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
