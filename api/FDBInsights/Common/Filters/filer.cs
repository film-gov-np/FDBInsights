namespace FDBInsights.Common.Filters;

public class filer : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var a = "asddsd";
        var b = "dfdfd";
        throw new NotImplementedException();
    }
}