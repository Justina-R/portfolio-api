public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
        _apiKey = Environment.GetEnvironmentVariable("PRIVATE_API_KEY")!;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Allow preflight CORS
        if (context.Request.Method == HttpMethods.Options)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("SECRET-API-KEY", out var extractedKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing");
            return;
        }

        if (extractedKey != _apiKey)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }
}
