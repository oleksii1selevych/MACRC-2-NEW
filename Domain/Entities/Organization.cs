using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("Organizations")]
    public class Organization
    {
        [Column("OrganizationId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Description { get; set; }
        public List<User> Users { get; set; } = null!;
        public List<Accident> Accidents { get; set; } = null!;
        public List<SmartBracelet> SmartBracelets { get; set; } = null!;
    }
}
