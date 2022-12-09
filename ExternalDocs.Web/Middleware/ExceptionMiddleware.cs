namespace ExternalDocs.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (TaskCanceledException)
            {

            }
            catch (ProblemException pe)
            {
                if (pe.Problem?.Status == StatusCodes.Status404NotFound)
                {
                    _logger.LogWarning(pe, "Ошибка при обратотке запроса. Информация: {@Problem}", pe.Problem);
                    await context.Response.SendRedirectAsync("/filenotfound", false);
                }
                else
                {
                    _logger.LogError(pe, "Ошибка при обратотке запроса. Информация: {@Problem}", pe.Problem);
                    await context.Response.SendRedirectAsync("/error", false);
                }
            }
        }
    }
}