using System.Timers;

namespace Marc2.Alert
{
    public class AlertService : IAlertService
    {
        private Dictionary<System.Timers.Timer, ResquerAlertDto> Alerts;
        public AlertService()
        {
            Alerts = new Dictionary<System.Timers.Timer, ResquerAlertDto>();
        }

        public void AddAlert(ResquerAlertDto resquerAlert)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 1 * 60 * 60 * 1000;
            timer.Elapsed += TimerElapsed;
            Alerts.Add(timer, resquerAlert);
            timer.Start();
        }

        public IEnumerable<ResquerAlertDto> GetAlerts(string userEmail, int organizationId)
        {
            var resquerAlerts = new List<ResquerAlertDto>();
            foreach (var alert in Alerts)
            {
                if (!alert.Value.UsersGotAlert.Contains(userEmail) && alert.Value.OrganizationId == organizationId) {
                    resquerAlerts.Add(alert.Value);
                    alert.Value.UsersGotAlert.Add(userEmail);
                }
            }
            return resquerAlerts;
        } 
        private void TimerElapsed(object? sender, ElapsedEventArgs e)
            => Alerts.Remove(sender as System.Timers.Timer);            
    }
}
