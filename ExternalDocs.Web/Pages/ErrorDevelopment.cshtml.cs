using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalDocs.Web.Pages
{
    public class ErrorDevelopmentModel : PageModel
    {
        [ViewData]
        public string Title => "Development error";

        public bool HasErrorInfo { get; private set; }

        public string ErrorMessage { get; private set; }

        public string StackTrace { get; private set; }

        public void OnGet([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return;
            }

            IExceptionHandlerFeature exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionHandlerFeature?.Error != null)
            {
                HasErrorInfo = true;
                ErrorMessage = exceptionHandlerFeature.Error.Message;
                StackTrace = exceptionHandlerFeature.Error.StackTrace;
            }
        }
    }
}
