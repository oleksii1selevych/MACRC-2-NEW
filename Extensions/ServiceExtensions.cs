using Marc2.Alert;
using Marc2.CertificateIssuerService;
using Marc2.Domain.Repositories;
using Marc2.Middleware;
using Marc2.Persistence;
using Marc2.Persistence.Repositories;
using Marc2.Services;
using Marc2.Services.Abstractions;
using Marc2.Tokens;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Marc2.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services)
           => services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureExceptionMiddleware(this IServiceCollection services)
            => services.AddTransient<ExceptionMiddleware>();

        public static void ConfigureTokenService(this IServiceCollection services)
            => services.AddTransient<ITokenService, TokenService>();


        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            const string customSchemeName = "JWT_OR_CERTIFICATE";

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = customSchemeName;
                options.DefaultChallengeScheme = customSchemeName;
                options.DefaultSignInScheme = customSchemeName;
            }).AddJwtBearer("Bearer", options =>
            {
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("secretKey")));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = signingKey
                };
            }).AddCertificate("Certificate", options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.Chained;
                options.RevocationMode = X509RevocationMode.NoCheck;

                options.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = context =>
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, context.ClientCertificate.Subject, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                            new Claim(ClaimTypes.Role, "IoT", ClaimValueTypes.String, context.Options.ClaimsIssuer)
                        };

                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                        context.Success();

                        return Task.CompletedTask;
                    }
                };
            }).AddPolicyScheme(customSchemeName, customSchemeName, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return "Bearer";

                    return "Certificate";
                };
            });
        }

        public static void ConfigureCertificateManagement(this IServiceCollection services)
        {
            services.AddCertificateManager();
            services.AddTransient<ICertificateIssuer, CertificateIssuer>();
        }

        public static void ConfigureAlertService(this IServiceCollection services)
            => services.AddSingleton<IAlertService, AlertService>();
    }
}
