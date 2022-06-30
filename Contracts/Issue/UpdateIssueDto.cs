using System.ComponentModel.DataAnnotations;

namespace Marc2.Contracts.Issue
{
    public class UpdateIssueDto
    {
        [MaxLength(500, ErrorMessage = "Issue text can not be longer than 500 characters")]
        public string Text { get; set; } = null!;
        public double? Lattitude { get; set; }
        public double? Longtitude { get; set; }
        public bool IsCompleted { get; set; }
        [MinLength(1)]
        public IEnumerable<int> WorkShiftIds { get; set; } = null!;
    }
}
