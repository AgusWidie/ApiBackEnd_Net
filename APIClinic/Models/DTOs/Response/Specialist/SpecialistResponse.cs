﻿namespace APIClinic.Models.DTOs.Response
{
    public class SpecialistResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
