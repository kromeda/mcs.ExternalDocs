WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddOptions();
builder.AddSeqLogger();

builder.Services.AddFastEndpoints();
builder.Services.AddRazorPages();
builder.Services.AddHttpClients();

WebApplication app = builder.Build();

app.AddExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseMiddleware<ExceptionMiddleware>();
app.UseFastEndpoints();
app.AddLostRedirectPage();

app.Run();