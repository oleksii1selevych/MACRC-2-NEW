namespace Marc2.Contracts.SmartBracelets
{
    public class UpdateBraceletDataDto
    {
        public int PulseRate { get; set; }
        public double Spo2Percentage { get; set; }
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
    }
}
