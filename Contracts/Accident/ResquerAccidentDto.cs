namespace Marc2.Contracts.Accident
{
    public class ResquerAccidentDto
    {
        public int AccidentId { get; set; }
        public string AccidentName { get; set; } = null!;
        public string GeneralDescription { get; set; } = null!;
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public string Address { get; set; } = null!;
        public DateTime AroseAt { get; set; }
    }
}
