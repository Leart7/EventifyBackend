using System.ComponentModel.DataAnnotations;

namespace EventifyWebApi.DTOs
{
    public class CreateLikeRequestDto
    {
        [Required]
        public int EventId { get; set; }
    }
}
