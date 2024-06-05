using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraSoft.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ErrorBase)
            {
                HandleProjectException(context);
            }
            else
            {
                Console.Write(context.Exception);
                ThrowUnkowError(context);
            }
        }

        private static void HandleProjectException(ExceptionContext context)
        {
            if (context.Exception is ModelValidateError)
            {
                var ex = (ModelValidateError)context.Exception;

                var errorResponse = new { ex.Errors };

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
            if (context.Exception is HandlerError)
            {
                var ex = (HandlerError)context.Exception;

                var errorResponse = new { ex.Errors };

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(errorResponse);
            }
            else
            {
                var errorResponse = new { Errors = context.Exception.Message };

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        private static void ThrowUnkowError(ExceptionContext context)
        {
            var errorResponse = new { Errors = new List<string> { "Unknow error" } };

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
