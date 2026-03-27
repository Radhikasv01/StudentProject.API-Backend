using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentProject.API
{
            public class ApiExceptionFilter : ExceptionFilterAttribute
        {
            private readonly Serilog.ILogger _logger;


            public ApiExceptionFilter(Serilog.ILogger logger)
            {

                _logger = logger;

            }
            public override void OnException(ExceptionContext context)
            {
                base.OnException(context);
            }

            public override Task OnExceptionAsync(ExceptionContext context)
            {
                //_logger.LogError(context.Exception, context.Exception.Message);
                //ProductModel exceptionModel;

                if (context == null)
                {
                }
                else
                {
                    _logger.Error(context.Exception, context.Exception.InnerException?.Message, context.Exception.StackTrace);
                }
                return base.OnExceptionAsync(context);

            }


        }
    }


