namespace ExternalDocs.Web.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAvaxCommunicator, AvaxCommunicator>();
            return services;
        }
    }
}
