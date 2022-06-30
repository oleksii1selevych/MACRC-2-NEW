using CertificateManager;
using CertificateManager.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Marc2.CertificateIssuerService
{
    public class CertificateIssuer : ICertificateIssuer
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly CreateCertificatesClientServerAuth _certificateCreator;
        private readonly ImportExportCertificate _certificateExporter;

        private const string country = "Ukraine";
        private const string organizationName = "EmeProtection";
        private const string dnsName = "localhost";

        public CertificateIssuer(IConfiguration configuration, IWebHostEnvironment environment, CreateCertificatesClientServerAuth certificateCreator,
            ImportExportCertificate certificateExporter)
        {
            _configuration = configuration;
            _environment = environment;
            _certificateCreator = certificateCreator;
            _certificateExporter = certificateExporter;
        }

        public void IssueDeviceCertificate(string manufacturerNumber, string certificatePassword, string hostingAddress)
        {
            var rootCA = GetRootCertificate();

            var domainName = new Uri(hostingAddress).Host;

            var deviceCertificate = _certificateCreator.NewDeviceChainedCertificate(
                new DistinguishedName { CommonName = manufacturerNumber },
                new ValidityPeriod() { ValidFrom = DateTime.Now, ValidTo = DateTime.Now.AddYears(5) },domainName, rootCA);

            deviceCertificate.FriendlyName = manufacturerNumber;

            var deviceCertificateInPfxBytes = _certificateExporter.ExportChainedCertificatePfx(certificatePassword, deviceCertificate, rootCA);
            SaveCertificate(deviceCertificateInPfxBytes, manufacturerNumber);
        }

        public string IssueRootCertificate()
        {
            var rootCertificate = _certificateCreator.NewRootCertificate(
                new DistinguishedName() { CommonName = "CA", Country = country, Organisation = organizationName },
                new ValidityPeriod() { ValidFrom = DateTime.Now, ValidTo = DateTime.Now.AddYears(5) },
                2, dnsName);

            rootCertificate.FriendlyName = "HTTPS development certificate";
            var password = GenerateRandomPassword();

            var rootCertificateInPfxBytes = _certificateExporter.ExportRootPfx(password, rootCertificate);
            SaveCertificate(rootCertificateInPfxBytes, "root");

            return password;
        }

        private void SaveCertificate(byte[] certificate, string fileName)
        {
            string wwwPath = _environment.WebRootPath;

            string path = Path.Combine(wwwPath, "certificates");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllBytes(Path.Combine(path, String.Format("{0}.pfx", fileName)), certificate);
        }

        public string IssueServerCetrificate()
        {
            var rootCA = GetRootCertificate();

            var serverCertificate = _certificateCreator.NewServerChainedCertificate(
                new DistinguishedName { CommonName = "server", Country = country, Organisation = organizationName },
                new ValidityPeriod() { ValidFrom = DateTime.Now, ValidTo = DateTime.Now.AddYears(5) },
                dnsName, rootCA);

            serverCertificate.FriendlyName = "HTTPS development certificate";
            var password = GenerateRandomPassword();

            var serverCertificateInPfxBytes = _certificateExporter.ExportChainedCertificatePfx(password, serverCertificate, rootCA);
            SaveCertificate(serverCertificateInPfxBytes, "server");

            return password;
        }

        private X509Certificate2 GetRootCertificate()
        {
            string wwwPath = _environment.WebRootPath;
            string path = Path.Combine(wwwPath, "certificates");

            string rootCertificateName = _configuration.GetValue<string>("RootCertificateName");
            string rootCertificatePassword = _configuration.GetValue<string>("RootCertificatePassword");

            var rootCA = new X509Certificate2(Path.Combine(path, rootCertificateName), rootCertificatePassword);
            return rootCA;
        }

        public string GenerateRandomPassword()
        {
            string generatedPassword = "";
            int randomValue;
            char letter;

            for (int i = 0; i < 32; i++)
            {

                randomValue = RandomNumberGenerator.GetInt32(0, 45);
                letter = Convert.ToChar(randomValue + 65);
                generatedPassword += letter;
            }

            return generatedPassword;
        }

        public byte[] GetCertificate(string manufacturerNumber)
        {
            string wwwPath = _environment.WebRootPath;
            string path = Path.Combine(wwwPath, "certificates");

            byte[] certificateBytes = File.ReadAllBytes(Path.Combine(path, String.Format("{0}.pfx", manufacturerNumber)));
            return certificateBytes;
        }

        public void DeleteCertificate(string manufacturerNumber)
        {
            string wwwPath = _environment.WebRootPath;
            string path = Path.Combine(wwwPath, "certificates");

            File.Delete(Path.Combine(path, String.Format("{0}.pfx", manufacturerNumber)));
        }
    }
}
