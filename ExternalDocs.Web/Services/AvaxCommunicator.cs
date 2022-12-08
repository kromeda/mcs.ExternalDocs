using Polly;
using Polly.Retry;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace ExternalDocs.Web.Services
{
    internal sealed class AvaxCommunicator : IAvaxCommunicator
    {
        private readonly HttpStatusCode[] _httpStatusCodesWorthRetry =
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };

        private readonly HttpClient _httpClient;
        private AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public AvaxCommunicator(HttpClient httpClient,
            IOptions<DocumentsConfiguration> configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.Value.Hosts.AvaxApi);
        }

        private AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =>
            _retryPolicy ??= Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(response => _httpStatusCodesWorthRetry.Contains(response.StatusCode))
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5)
            });

        public async Task<FileDocumentView> GetNotificationFile(bool physic, Guid token, CancellationToken ct)
        {
            string route = $"api/external/{physic}/{token}";
            HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(route, ct));
            return await ReadContent<FileDocumentView>(response, ct);
        }

        public async Task<FileDocumentView> GetNotificationFile(bool physic, string token, CancellationToken ct)
        {
            string route = $"api/external/{physic}/file/{token}";
            HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(route, ct));
            return await ReadContent<FileDocumentView>(response, ct);
        }

        private static async Task<T> ReadContent<T>(HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
            {
                MediaTypeFormatter[] formatters = new[] { new ProblemDetailsMediaTypeFormatter() };
                ProblemDetails problem = await response.Content.ReadAsAsync<ProblemDetails>(formatters, ct);
                throw new ProblemException(problem);
            }

            return await response.Content.ReadAsAsync<T>(ct);
        }

        private class ProblemDetailsMediaTypeFormatter : JsonMediaTypeFormatter
        {
            public ProblemDetailsMediaTypeFormatter()
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            }
        }
    }
}