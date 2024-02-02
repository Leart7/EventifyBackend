using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class DeleteRequestDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
