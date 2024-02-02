using EventifyCommon.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventifyWebApi.DTOs
{
    public class CreateEventRequestDto
    {
        [Required]
        [MaxLength(75)]
        public string Title { get; set; }
        [Required]
        [MaxLength(140)]
        public string Description { get; set; }
        [Required]
        public string Organizer { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? City { get; set; }
        public string? About { get; set; }
        public double? Price { get; set; }
        public int? Capacity { get; set; }
        public bool? EndSales { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int? CurrencyId { get; set; }
        [Required]
        public int FormatId { get; set; }
        [Required]
        public int LanguageId { get; set; }
        [Required]
        public int TypeId { get; set; }
        [SwaggerSchema("Array of image files")]
        public ICollection<IFormFile>? Images { get; set; }
        public List<EventAgendDto>? EventAgends { get; set; }
        public List<string>? Tags { get; set; }
    }
}
