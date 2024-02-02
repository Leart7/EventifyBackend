using EventifyCommon.Models.AbstractModels;

namespace EventifyCommon.Models
{
    public class Image : BaseModel
    {
        public string ImageUrl { get; set; }
        public int EventId { get; set; }

        // Navigation property
        public virtual Event Event { get; set; }

    }
}
