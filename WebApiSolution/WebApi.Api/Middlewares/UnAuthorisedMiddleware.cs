using System.Net;

namespace WebApi.Api.Middlewares
{
    public class UnAuthorisedMiddleware
    {

        private readonly RequestDelegate _next;

        public UnAuthorisedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized && !context.Response.HasStarted)
            {
                // Write the custom response message directly
                context.Response.ContentType = "text/plain";
                context.Response.Headers.Append("UnAuthorised", "401");
                await context.Response.WriteAsync("UnAuthorised!\nYou are not Authorized to Access this Endpoint!! \nPlease Ensure you have necessary permissions!!!");
            }
        }
    }

}

