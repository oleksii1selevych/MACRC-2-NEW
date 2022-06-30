using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("Assignments")]
    public class Assignment
    {
        [Key]
        [Column("AssignmentId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IssueId { get; set; }
        public Issue Issue { get; set; } = null!;
        [ForeignKey(nameof(WorkShift))]
        public int WorkShiftId { get; set; }
        public WorkShift WorkShift { get; set; } = null!;
    }
}
