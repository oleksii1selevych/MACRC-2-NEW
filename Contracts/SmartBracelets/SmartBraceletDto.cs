namespace Marc2.Contracts.SmartBracelets
{
    public class SmartBraceletDto
    {
        public int SmartBraceletId { get; set; }
        public string ManufacturerCode { get; set; } = null!;
        public string HostingAddress { get; set; } = null!;
        public string CertificatePassword { get; set; } = null!;
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}
