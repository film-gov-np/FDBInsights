namespace FDBInsights.Data;

public interface ISubscribeTableDependency
{
    void SubscribeTableDependency(string? connectionString);
}