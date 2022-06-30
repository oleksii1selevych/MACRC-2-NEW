using System.Timers;

namespace Marc2.Alert
{
    public enum AlertTypes
    {
        Emergence,
        State,
        NoConnection
    }

    public class ResquerAlertDto
    {
        public AlertTypes AlertType { get; set; }
        public DateTime AlertedAt { get; set; }
        public int PulseRate { get; set; }
        public double Spo2Percentage { get; set; }
        public int WorkShiftId { get; set; }
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public string ResquerFullName { get; set; } = null!;
        public int OrganizationId { get; set; }
        public List<string> UsersGotAlert;

        public ResquerAlertDto()
        {
            this.UsersGotAlert = new List<string>();
            this.AlertedAt = DateTime.Now;
        }
    }
}
