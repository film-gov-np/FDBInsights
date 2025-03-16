using FastEndpoints;

namespace FDBInsights.Common.Filters;

public class AuthorizationPreProcessor : IPreProcessor
{
    // private readonly IAuthService _authService;
    // private readonly IJwtRepository _jwtRepository;
    //
    // public AuthorizationPreProcessor(IAuthService authService, IJwtRepository jwtRepository)
    // {
    //     _authService = authService;
    //     _jwtRepository = jwtRepository;
    // }

    public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
        // Your authorization logic here
        // You can use ctx.HttpContext to access the request

        // If authorization fails:
        // ctx.HttpContext.Response.StatusCode = 401;
        // await ctx.HttpContext.Response.WriteAsync("Unauthorized", ct);
        // ctx.HttpContext.Response.ContentType = "text/plain";
        // ctx.HttpContext.Response.StatusCode = 401;
        // ctx.HttpContext.SetEndpointResult(Results.Unauthorized());
    }
}