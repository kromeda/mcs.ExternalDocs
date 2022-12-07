WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddOptions();
builder.AddSeqLogger();

builder.Services.AddRazorPages();
builder.Services.AddApplicationInsightsTelemetry();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

string[] defaultRoutes = { "/", "/index" };

app.Use(async (context, next) =>
{
    if (defaultRoutes.Contains(context.Request.Path.Value))
    {
        context.Response.Redirect("https://kuban.tns-e.ru");
        return;
    }

    await next();
});

app.Run();
