namespace ExternalDocs.Web.Endpoints
{
    public class FileByShortLinkEndpoint : Endpoint<NotificationRequest<string>, FileDocument>
    {
        public IAvaxCommunicator Communicator { get; set; }

        public override void Configure()
        {
            Get("/{physical:regex([01])}/{token:regex(^[a-zA-Z0-9]{{6,}}$)}");
            AllowAnonymous();
            PostProcessors(new MarkInlineHeader<NotificationRequest<string>, FileDocument>());
        }

        public override async Task HandleAsync(NotificationRequest<string> request, CancellationToken ct)
        {
            FileDocument doc = await Communicator.GetNotificationFile(request.IsPhysical, request.Token, ct);

            if (doc == null)
            {
                Logger.LogWarning("Файл не найден. Идентификатор: {Token}", request.Token);
                await SendRedirectAsync("/filenotfound", cancellation: ct);
            }
            else
            {
                Response = doc;
                await SendBytesAsync(doc.Data, contentType: "application/pdf", cancellation: ct);
                Logger.LogInformation("Запрошен файл, название: {FileName}, по идентификатору: {Token}.", doc.Name, request.Token);
            }
        }
    }
}
