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
        await FindFile(async communicator => await communicator.GetNotificationFile(physical == 1, token, ct), token))
    .AddFileFilters();

app.MapGet("/n/{physical:regex([01])}/{token:guid}",
    async (int physical, Guid token, CancellationToken ct) =>
        await FindFile(async communicator => await communicator.GetNotificationFile(physical == 1, token, ct), token.ToString()))
    .AddFileFilters();

app.Run();

async Task<IResult> FindFile(Func<IAvaxCommunicator, Task<FileDocumentView>> handler, string token)
{
    IAvaxCommunicator communicator = app.Services.GetRequiredService<IAvaxCommunicator>();
    ILogger<Program> logger = app.Services.GetRequiredService < ILogger<Program>>();

    FileDocumentView doc = await handler.Invoke(communicator);

    if (doc == null)
    {
        logger.LogWarning("Файл не найден. Идентификатор: {Token}", token);
        return Results.Redirect("/filenotfound");
    }
    else
    {
        logger.LogInformation("Запрошен файл \"{FileName}\" по идентификатору {Token}", doc.Name, token);
        return Results.File(doc.Data, contentType: "application/pdf");
    }
}