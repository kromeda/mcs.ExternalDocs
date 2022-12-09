namespace ExternalDocs.Web.Models.Requests
{
    public class NotificationRequest<T> where T : notnull
    {
        public T Token { get; init; } = default;

        public int? Physical { get; init; } = default;

        public bool IsPhysical => Physical.HasValue && Physical.Value == 1;
    }
}