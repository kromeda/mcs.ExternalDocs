namespace ExternalDocs.Web.Extensions
{
    internal static class WebApplicationExtensions
    {
        public static void AddExceptionPage(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
        }
    }
}
