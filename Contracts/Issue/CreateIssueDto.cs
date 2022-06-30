using System.ComponentModel.DataAnnotations;

namespace Marc2.Contracts.Issue
{
    public class CreateIssueDto
    {
        [MaxLength(500, ErrorMessage = "Issue text can not be longer than 500 characters")]
        public string Text { get; set; } = null!;
        public double? Lattitude { get; set; }
        public double? Longtitude { get; set; }
        public int AccidentId { get; set; }
        [MinLength(1)]
        public IEnumerable<int> WorkShiftIds { get; set; } = null!;
    }
}
