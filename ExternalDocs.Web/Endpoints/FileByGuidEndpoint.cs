namespace ExternalDocs.Web.Endpoints
{
    public class FileByGuidEndpoint : Endpoint<NotificationRequest<Guid>, FileDocument>
    {
        public IAvaxCommunicator Communicator { get; set; }

        public override void Configure()
        {
            Get("/n/{physical:regex([01])}/{token:guid}");
            AllowAnonymous();
            PostProcessors(new MarkInlineHeader<NotificationRequest<Guid>, FileDocument>());
        }

        public override async Task HandleAsync(NotificationRequest<Guid> request, CancellationToken ct)
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
                Logger.LogInformation("Запрошен файл \"{FileName}\" по идентификатору {Token}", doc.Name, request.Token);
            }
        }
    }
}