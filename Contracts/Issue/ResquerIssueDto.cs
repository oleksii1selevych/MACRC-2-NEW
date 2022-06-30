namespace Marc2.Contracts.Issue
{
    public class ResquerIssueDto
    {
        public int IssueId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
