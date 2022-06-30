namespace Marc2.Alert
{
    public interface IAlertService
    {
        IEnumerable<ResquerAlertDto> GetAlerts(string userEmail, int organizationId);
        void AddAlert(ResquerAlertDto resquerAlert);
    }
}
