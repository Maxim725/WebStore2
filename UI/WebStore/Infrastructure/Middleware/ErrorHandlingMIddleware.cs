using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMIddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMIddleware> _logger;
        public ErrorHandlingMIddleware(RequestDelegate next, ILogger<ErrorHandlingMIddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                HandleError(context, ex);
            }
        }

        private void HandleError(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке запроса {0}", context.Request.Path );
        }
    }
}
