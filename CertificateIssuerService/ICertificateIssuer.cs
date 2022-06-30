namespace Marc2.CertificateIssuerService
{
    public interface ICertificateIssuer
    {
        string IssueRootCertificate();
        string IssueServerCetrificate();
        void IssueDeviceCertificate(string manufacturerNumber, string certificatePassword, string hostingAddress);
        byte[] GetCertificate(string manufacturerNumber);
        void DeleteCertificate(string manufacturerNumber);
        string GenerateRandomPassword();
    }
}
