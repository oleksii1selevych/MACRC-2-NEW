using System.ComponentModel.DataAnnotations;

namespace Marc2.Contracts.Accident
{
    public class CreateAccidentDto
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Accident description can not be longer than 1000 characters")]
        public string GeneralDescription { get; set; } = null!;
        [Required]
        [StringLength(100, ErrorMessage = "Accident name can not be longer than 100 characters")]
        public string AccidentName { get; set; } = null!;
        [Required]
        public double Lattitude { get; set; }
        [Required]
        public double Longtitude { get; set; }
        public string Address { get; set; } = null!;
    }
}
