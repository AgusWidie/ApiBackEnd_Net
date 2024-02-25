namespace APIClinic.Models.DTOs.Response
{
    public class TransactionHeaderPatientResponse
    {
        public long Id { get; set; }

        public long? ClinicId { get; set; }

        public string ClinicName { get; set; } = "";

        public long? BranchId { get; set; }

        public string BranchName { get; set; } = "";

        public string TransactionNo { get; set; } = "";

        public DateTime? TransactionDate { get; set; }

        public long ExaminationDoctorId { get; set; }

        public long? DoctorId { get; set; }

        public string DoctorName { get; set; } = "";

        public string QueueNo { get; set; } = "";

        public string Ktpno { get; set; } = "";

        public string FamilyCardNo { get; set; } = "";

        public string Name { get; set; } = "";

        public string PaymentType { get; set; } = "";

        public string Bpjsno { get; set; } = "";

        public string InsuranceName { get; set; } = "";

        public string InsuranceNo { get; set; } = "";

        public long? Total { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
