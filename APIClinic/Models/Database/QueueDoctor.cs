﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace APIClinic.Models.Database
{
    public partial class QueueDoctor
    {
        public long Id { get; set; }
        public long? SpecialistDoctorId { get; set; }
        public string QueueNo { get; set; }
        public DateTime QueueDate { get; set; }
        public int? IsPrint { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}