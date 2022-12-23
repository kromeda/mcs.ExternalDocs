namespace ExternalDocs.Web.Pages
{
    public class PageNotFoundModel : PageModel
    {
        private readonly ILogger<PageNotFoundModel> _logger;

        public PageNotFoundModel(ILogger<PageNotFoundModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogWarning("Страница не найдена.");
        }
    }
}
