namespace APIClinic.Models.DTOs.Response
{
    public class DrugResponse
    {
        public long Id { get; set; }

        public long? ClinicId { get; set; }

        public string ClinicName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public string DrugName { get; set; } = "";

        public string UnitType { get; set; } = "";

        public long? Price { get; set; }

        public long? Stock { get; set; }

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
