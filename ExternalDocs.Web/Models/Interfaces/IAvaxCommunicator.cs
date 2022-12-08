namespace ExternalDocs.Web.Models.Interfaces
{
    internal interface IAvaxCommunicator
    {
        Task<FileDocumentView> GetNotificationFile(bool physic, Guid token, CancellationToken ct);

        Task<FileDocumentView> GetNotificationFile(bool physic, string token, CancellationToken ct);
    }
}
