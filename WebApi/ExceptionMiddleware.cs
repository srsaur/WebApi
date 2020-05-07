using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response
                      .WriteAsync(JsonConvert.SerializeObject(new JObject(
                          new JProperty("success", false),
                          new JProperty("message", ex.Message),
                          new JProperty("detail", ex.InnerException?.ToString() ?? ex.StackTrace))));
            }
        }
    }
}
