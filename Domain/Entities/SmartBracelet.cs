using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("SmartBracelets")]
    public class SmartBracelet
    {
        [Column("SmartBraceletId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(19)]
        public string ManufacturerCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? PulseRate { get; set; }
        public double? Spo2Percentage { get; set; }
        public double? Lattitude { get; set; }
        public double? Logngtitude { get; set; }
        public string CertificatePassword { get; set; } = null!;
        public string HostAddress { get; set; } = null!;
        public DateTime? LastRequest { get; set; }
        [ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
    }
}
