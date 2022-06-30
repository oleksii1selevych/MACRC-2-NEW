namespace Marc2.Contracts.WorkShifts
{
    public class WorkShiftDto
    {
        public int WorkShiftId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public int PulseRate { get; set; }
        public double Spo2Percentage { get; set; }
    }
}
