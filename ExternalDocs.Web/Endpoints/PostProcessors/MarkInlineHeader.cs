using FluentValidation.Results;
using Microsoft.Net.Http.Headers;

namespace ExternalDocs.Web.Endpoints.PostProcessors
{
    public class MarkInlineHeader<TRequest, TResponse> : IPostProcessor<TRequest, TResponse>
        where TResponse : FileDocument
    {
        public Task PostProcessAsync(TRequest req, TResponse res, HttpContext ctx, IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
        {
            if (res != null)
            {
                string headerValue = $"inline; filename={res.Name}; filename*=UTF-8''{res.Name}";
                ctx.Response.Headers.Add(HeaderNames.ContentDisposition, headerValue);
            }

            return Task.CompletedTask;
        }
    }
}
