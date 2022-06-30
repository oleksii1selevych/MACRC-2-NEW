using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;

namespace Marc2.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static void ConfigureKestrelOptions(this ConfigureWebHostBuilder webHost, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "certificates");

            string serverCertificateName = configuration.GetValue<string>("ServerCertificateName");
            string serverCertificatePassword = configuration.GetValue<string>("ServerCertificatePassword");

            var certificate = new X509Certificate2(Path.Combine(path, serverCertificateName), serverCertificatePassword);

            webHost.ConfigureKestrel(kestrelOptions =>
            {
                kestrelOptions.ConfigureHttpsDefaults(options =>
                {
                    options.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                    options.ServerCertificate = certificate;
                });
            });
        }
    }
}
