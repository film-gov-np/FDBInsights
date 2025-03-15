namespace FDBInsights.Common;

public record ApiResponse<T>(
    T? Data,
    string? Message = null,
    int StatusCode = 200,
    bool Success = true
);