using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("Issues")]
    public class Issue
    {
        [Key]
        [Column("IssueId")]
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public double? Lattitude { get; set; }
        public double? Longtitude { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(Accident))]
        public int AccidentId { get; set; }
        public Accident Accident { get; set; } = null!;
        public List<Assignment> Assignments { get; set; } = null!;
    }
}
