﻿namespace APIClinic.Models.DTOs.Request
{
    public class PatientRegistrationRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string? QueueNo { get; set; } = "";

        public long SpecialistDoctorId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Ktpno { get; set; } = "";

        public string? FamilyCardNo { get; set; } = "";

        public string Name { get; set; } = "";

        public DateOnly? DateOfBirth { get; set; }

        public string Gender { get; set; } = "";

        public string Address { get; set; } = "";

        public string? Religion { get; set; } = "";

        public string? Education { get; set; } = "";

        public string? Work { get; set; } = "";

        public string Complaint { get; set; } = "";

        public string PaymentType { get; set; } = "";

        public string? Bpjsno { get; set; } = "";

        public string? InsuranceName { get; set; } = "";

        public string? InsuranceNo { get; set; } = "";

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
