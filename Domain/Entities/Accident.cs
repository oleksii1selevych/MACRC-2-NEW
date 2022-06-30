using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("Accidents")]
    public class Accident
    {
        [Column("AccidentId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string GeneralDescription { get; set; } = null!;
        public string AccidentName { get; set; } = null!;
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public string Address { get; set; } = null!;
        [ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public DateTime AroseAt  { get; set; }
    }
}
