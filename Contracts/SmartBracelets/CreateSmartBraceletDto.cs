namespace Marc2.Contracts.SmartBracelets
{
    public class CreateSmartBraceletDto
    {
        public string ManufacturerCode { get; set; } = null!;
        public string HostingAddress { get; set; } = null!;
        public int OrganizationId { get; set; }
    }
}
