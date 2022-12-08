namespace ExternalDocs.Web.Extensions
{
    internal static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder AddFileFilters(this RouteHandlerBuilder builder)
        {
            builder.AddEndpointFilter<ProblemDetailsFilter>();
            builder.AddEndpointFilter<AddHeadersFilter>();

            return builder;
        }
    }
}
