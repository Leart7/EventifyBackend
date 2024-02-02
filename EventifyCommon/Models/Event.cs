using EventifyCommon.Models.AbstractModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyCommon.Models
{
    public class Event : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Organizer { get; set; }
        public string? UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? About { get; set; }
        public double? Price { get; set; }
        public int? Capacity { get; set; }
        public string? City { get; set; }
        public bool? EndSales { get; set; }
        public string? Status { get; set; }
        public int CategoryId { get; set; }
        public int? CurrencyId { get; set; }
        public int FormatId { get; set; }
        public int LanguageId { get; set; }
        public int TypeId { get; set; }

        //Navigation properties
        public Category? Category { get; set; }
        public Currency? Currency { get; set; }
        public Format? Format { get; set; }
        public Language? Language { get; set; }
        public Type? Type { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<EventAgend>? EventAgends { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<ReportEvent>? Reports { get; set; }


    }
}
