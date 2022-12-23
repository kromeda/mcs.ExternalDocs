WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddOptions();
builder.AddSeqLogger();

builder.Services.AddFastEndpoints();
builder.Services.AddFixedWindowRateLimiter(builder);

builder.Services.AddRazorPages();
builder.Services.AddHttpClients();

WebApplication app = builder.Build();

app.UseRateLimiter();
app.AddExceptionPage();
app.UseStaticFiles();
app.UseAuthorization();
app.MapRazorPages();
app.UseMiddleware<ExceptionMiddleware>();
app.UseFastEndpoints().UseRateLimiter();
app.AddLostRedirectPage();

app.Run();
