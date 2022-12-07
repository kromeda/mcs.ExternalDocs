namespace ExternalDocs.Web.Extensions
{
    internal static class WebApplicationBuilderExtensions
    {
        public static void AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Configuration.AddJsonFile($"appsettings.{Utility.CurrentEnvironment}.json");
        }

        public static void AddSeqLogger(this WebApplicationBuilder builder)
        {
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddSerilog();

            DocumentsConfiguration configurations = builder
                .Configuration
                .GetSection(nameof(DocumentsConfiguration))
                .Get<DocumentsConfiguration>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Seq(configurations.SeqHttpHost, apiKey: configurations.SeqApiKey)
                .CreateLogger();
        }
    }
}