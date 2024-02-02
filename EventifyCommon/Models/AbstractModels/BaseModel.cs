using System.ComponentModel.DataAnnotations;

namespace EventifyCommon.Models.AbstractModels
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created_at { get; set; } = DateTime.Now;

        public DateTime Updated_at { get; set; } = DateTime.Now;

    }
}
