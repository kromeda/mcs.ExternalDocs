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

        public static void AddLostRedirectPage(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                await next(context);
                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/pagenotfound");
                    ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
            
                    logger.LogWarning("Страница не найдена.");
                }
            });
        }
    }
}
