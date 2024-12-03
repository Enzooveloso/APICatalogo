using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filter
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public readonly ILogger<ApiExceptionFilter> _logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu um exceçao nao tratada");
            context.Result = new ObjectResult("Ocoru um problema ao trratar a sua solicitação")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };

        }
    }
}
