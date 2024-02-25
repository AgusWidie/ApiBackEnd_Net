﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace APIClinic.Models.Database
{
    public partial class TransactionLab
    {
        public long Id { get; set; }
        public long? ClinicId { get; set; }
        public long? BranchId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public long? ExaminationLabId { get; set; }
        public string PaymentType { get; set; }
        public string Bpjsno { get; set; }
        public string InsuranceNo { get; set; }
        public string InsuranceName { get; set; }
        public long? Total { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Clinic Clinic { get; set; }
        public virtual ExaminationLab ExaminationLab { get; set; }
    }
}