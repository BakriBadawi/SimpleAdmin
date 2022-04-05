using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;


public class AutoLog : TypeFilterAttribute
{
    public AutoLog() : base(typeof(AutoLogImpl))
    {

    }

    private class AutoLogImpl : IActionFilter, IExceptionFilter
    {
        private readonly ILogger _logger;
        public AutoLogImpl(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AutoLog>();
        }
        private string logString="";
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logString = GetLogString(context);
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.Log(LogLevel.Information, logString);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.Log(LogLevel.Warning, context.Exception, logString);
                }
                return;
            }
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.Log(LogLevel.Error, context.Exception, logString);
            }
            context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            return;
        }
        private string GetLogString(FilterContext context)
        {
            if (!context.HttpContext.Request.Body.CanSeek)
            {
                context.HttpContext.Request.EnableBuffering();
            }


            string requestUrl = context.HttpContext.Request.GetDisplayUrl();
            string requestBody = "";
            using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
            {
                context.HttpContext.Request.Body.Position = 0;
                requestBody = reader.ReadToEndAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                context.HttpContext.Request.Body.Position = 0;
            }
            return $"Method:{context.HttpContext.Request.Method}{Environment.NewLine} RequestUrl:{requestUrl}{Environment.NewLine}RequestBody:{requestBody}";
        }
    }
}
