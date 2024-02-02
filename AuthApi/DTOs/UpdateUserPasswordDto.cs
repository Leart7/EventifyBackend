using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class UpdateUserPasswordDto
    {
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }
}
