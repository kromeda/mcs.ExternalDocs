using FluentValidation.Results;
using Microsoft.Net.Http.Headers;

namespace ExternalDocs.Web.Endpoints.PostProcessors
{
    public class MarkInlineHeader<TRequest> : IPreProcessor<TRequest>
    {
        public Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
        {
            string headerValue = $"inline; filename=restriction.pdf; filename*=UTF-8''restriction.pdf";
            ctx.Response.Headers.Add(HeaderNames.ContentDisposition, headerValue);

            return Task.CompletedTask;
        }
    }
}