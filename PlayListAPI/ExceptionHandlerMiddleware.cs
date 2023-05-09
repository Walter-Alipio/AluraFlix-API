namespace PlayListAPI
{
  public class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext httpContext)
    {
      try
      {
        httpContext.Request.Headers.TryGetValue("Authorization", out var key);
        // System.Console.WriteLine(key);

        await _next(httpContext);
      }
      catch (Exception exception)
      {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";
        System.Console.WriteLine(exception.Message);
      }
    }

  }
}