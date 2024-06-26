﻿
using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Web.Models.Dto
{
    public class VillaUpdateDto
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public int SqFt { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
    }
}
