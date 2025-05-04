﻿namespace HRMS.API.Models
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public LocationDto? Location { get; set; }
    }
}
