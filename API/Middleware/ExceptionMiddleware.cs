using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ILogger<ExceptionMiddleware> _Logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env )
        {
            _env = env;
            _Logger = logger;
            _next = next;
        }

        public async Task InvokeAsync (HttpContext context){

          
          try{
                await _next(context);
          }catch(Exception Ex){
             _Logger.LogError(Ex , Ex.Message);
             context.Response.ContentType="application/json";
             context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
             var response = _env.IsDevelopment() 
             ? new ApiException((int)HttpStatusCode.InternalServerError , Ex.Message , Ex.StackTrace.ToString() )
             : new ApiException((int) HttpStatusCode.InternalServerError);
          
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; 
            var json = JsonSerializer.Serialize(response,options);

            await context.Response.WriteAsync(json);
          }
        }
    }
}