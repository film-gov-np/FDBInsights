namespace FDBInsights.Common;

public record ApiResponse<T>(
    T? Data,
    string? Message = "",
    int StatusCode = 200,
    bool Success = true
);