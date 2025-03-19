namespace FDBInsights.Common.Filter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequiredRolesAttribute(params string[] roles) : Attribute
{
    public string[] Roles { get; } = roles;
}