namespace ExternalDocs.Web.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAvaxCommunicator, AvaxCommunicator>();
            return services;
        }

        public static IServiceCollection AddFixedWindowRateLimiter(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.Configure<FixedWindowConfiguration>(builder.Configuration.GetSection(FixedWindowConfiguration.Name));

            FixedWindowConfiguration configuration = new();
            builder.Configuration.GetSection(FixedWindowConfiguration.Name).Bind(configuration);

            services.AddRateLimiter(o =>
            {
                o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
                {
                    IPAddress address = context.Connection.RemoteIpAddress;

                    return RateLimitPartition.GetFixedWindowLimiter(address, x => new FixedWindowRateLimiterOptions
                    {
                        QueueLimit = configuration.QueueLimit,
                        PermitLimit = configuration.PermitLimit,
                        Window = TimeSpan.FromSeconds(configuration.Window)
                    });
                });
                o.OnRejected = async (ctx, token) =>
                {
                    ILogger<Program> logger = ctx.HttpContext.Resolve<ILogger<Program>>();
                    logger.LogWarning("Слишком много запросов от клиента {Ip}", ctx.HttpContext.Connection.RemoteIpAddress);

                    StaticResolver resolver = ctx.HttpContext.Resolve<StaticResolver>();
                    string pageContent = await resolver.GetMarkup("TooManyRequests.html");
                    await ctx.HttpContext.Response.WriteAsync(pageContent, Encoding.UTF8, token);
                };
            });

            return services;
        }
    }
}
