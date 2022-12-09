namespace ExternalDocs.Web.Models.Interfaces
{
    public interface IAvaxCommunicator
    {
        Task<FileDocument> GetNotificationFile(bool physic, Guid token, CancellationToken ct);

        Task<FileDocument> GetNotificationFile(bool physic, string token, CancellationToken ct);
    }
}
