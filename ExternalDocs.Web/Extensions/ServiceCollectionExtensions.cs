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
                o.OnRejected = (ctx, token) =>
                {
                    ILogger<Program> logger = ctx.HttpContext.Resolve<ILogger<Program>>();
                    logger.LogWarning("Too many requests from client [{Ip}]", ctx.HttpContext.Connection.RemoteIpAddress);
                    return ValueTask.CompletedTask;
                };
            });

            return services;
        }
    }
}
