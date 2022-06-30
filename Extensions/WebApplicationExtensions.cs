using Marc2.Middleware;

namespace Marc2.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseExceptionMiddleware(this WebApplication app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}
