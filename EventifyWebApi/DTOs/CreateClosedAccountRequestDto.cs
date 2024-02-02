using System.ComponentModel.DataAnnotations;

namespace EventifyWebApi.DTOs
{
    public class CreateClosedAccountRequestDto
    {
        [Required]
        public int ClosedAccountReasonId { get; set; }
        public string? Description { get; set; }
    }
}
