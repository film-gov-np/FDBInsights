namespace FDBInsights.Models;

public class UserRole
{
    public int RoleID { get; init; }

    public string Name { get; init; }

    public bool? DefaultRole { get; init; }
    public string Remarks { get; init; }
    public string InsertedBy { get; init; }
    public DateTime InsertedDate { get; init; }
    public string UpdatedBy { get; init; }

    public DateTime? UpdatedDate { get; init; }
}