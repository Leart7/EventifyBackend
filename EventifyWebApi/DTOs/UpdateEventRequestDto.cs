using EventifyCommon.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventifyWebApi.DTOs
{
    public class UpdateEventRequestDto
    {
        [MaxLength(75)]
        public string? Title { get; set; }
        [MaxLength(140)]
        public string? Description { get; set; }
        public string? Organizer { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? City { get; set; }
        public string? About { get; set; }
        public double? Price { get; set; }
        public int? Capacity { get; set; }
        public bool? EndSales { get; set; }
        public int? CategoryId { get; set; }
        public int? CurrencyId { get; set; }
        public int? FormatId { get; set; }
        public int? LanguageId { get; set; }
        public int? TypeId { get; set; }
        [SwaggerSchema("Array of image files")]
        public ICollection<IFormFile>? Images { get; set; }
        public List<EventAgend>? EventAgends { get; set; }
        public List<string>? Tags { get; set; }
    }
}
