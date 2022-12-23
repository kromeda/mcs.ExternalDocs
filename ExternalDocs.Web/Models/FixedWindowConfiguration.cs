namespace ExternalDocs.Web.Models
{
    public class FixedWindowConfiguration
    {
        public const string Name = nameof(FixedWindowConfiguration);

        public int QueueLimit { get; init; } = 0;

        public int PermitLimit { get; init; } = 20;

        public int Window { get; init; } = 120;
    }
}
