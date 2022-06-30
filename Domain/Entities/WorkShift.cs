using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("WorkShifts")]
    public class WorkShift
    {
        [Key]
        [Column("WorkShiftId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [ForeignKey(nameof(SmartBracelet))]
        public int SmartBraceletId { get; set; }
        public SmartBracelet SmartBracelet { get; set; } = null!;
        public List<Assignment> Assignments { get; set; } = null!;
    }
}
