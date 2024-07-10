﻿using Journey.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Journey.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is JourneyException)
            {
                var JourneyException = (JourneyException)context.Exception;
                context.HttpContext.Response.StatusCode = (int)JourneyException.GetStatusCode();
                context.Result = new ObjectResult(JourneyException.Message);
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Result = new ObjectResult("Erro desconhecido");
            }
        }
    }
}
