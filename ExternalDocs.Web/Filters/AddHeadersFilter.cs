using Microsoft.Net.Http.Headers;

namespace ExternalDocs.Web.Filters
{
    public class AddHeadersFilter : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            string headerValue = "inline; filename=restriction.pdf; filename*=UTF-8''restriction.pdf";
            context.HttpContext.Response.Headers.Add(HeaderNames.ContentDisposition, headerValue);
            return await next(context);
        }
    }
}