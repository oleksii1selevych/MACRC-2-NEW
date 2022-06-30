using System.ComponentModel.DataAnnotations;

namespace Marc2.Contracts.User
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(40, ErrorMessage = "First Name can't be more than 40 characters")]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(40, ErrorMessage = "Last Name can't be more than 40 characters")]
        public string LastName { get; set; } = null!;
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided phone number is not in appropriate format")]
        public string? PhoneNumber { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = " You must provide at least one role for the user")]
        public IEnumerable<int> UserRoles { get; set; } = null!;
    }
}
