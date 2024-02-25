﻿namespace APIClinic.Models.DTOs.Request
{
    public class SpecialistSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public string Name { get; set; } = "";
    }
}
