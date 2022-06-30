using System.ComponentModel.DataAnnotations;

namespace Marc2.Contracts.User
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = null!;
    }
}
