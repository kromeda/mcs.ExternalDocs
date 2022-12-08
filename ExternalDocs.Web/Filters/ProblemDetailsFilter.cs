namespace ExternalDocs.Web.Filters
{
    public class ProblemDetailsFilter : IEndpointFilter
    {
        private readonly ILogger<ProblemDetailsFilter> _logger;

        public ProblemDetailsFilter(ILogger<ProblemDetailsFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                return await next(context);
            }
            catch (ProblemException pe)
            {
                _logger.LogWarning(pe, "Ошибка при обратотке запроса. Title: {Title}; Detail: {Detail}", pe.Problem.Title, pe.Problem.Detail);
                return Results.Redirect("/filenotfound");
            }
        }
    }
}
