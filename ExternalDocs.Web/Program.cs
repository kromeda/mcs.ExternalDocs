WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddOptions();
builder.AddSeqLogger();

builder.Services.AddRazorPages();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClients();

WebApplication app = builder.Build();

app.AddExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Use(async (context, next) =>
{
    await next(context);
    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/pagenotfound");
        return;
    }
});

app.MapGet("/{physical:regex([01])}/{token:regex(^[a-zA-Z0-9]{{6,}}$)}",
    async (int physical, string token, CancellationToken ct) =>
        await FindFile(async communicator => await communicator.GetNotificationFile(physical == 1, token, ct)))
    .AddFileFilters();

app.MapGet("/n/{physical:regex([01])}/{token:guid}",
    async (int physical, Guid token, CancellationToken ct) =>
        await FindFile(async communicator => await communicator.GetNotificationFile(physical == 1, token, ct)))
    .AddFileFilters();

app.Run();

async Task<IResult> FindFile(Func<IAvaxCommunicator, Task<FileDocumentView>> handler)
{
    IAvaxCommunicator communicator = app.Services.GetRequiredService<IAvaxCommunicator>();
    FileDocumentView doc = await handler.Invoke(communicator);

    IResult result = doc == null
        ? Results.Redirect("/filenotfound")
        : Results.File(doc.Data, contentType: "application/pdf");

    return result;
}