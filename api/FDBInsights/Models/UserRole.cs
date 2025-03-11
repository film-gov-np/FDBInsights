namespace FDBInsights.Models;

public class UserRole
{
    public int RoleID { get; set; }

    public string Name { get; set; }

    public bool? DefaultRole { get; set; }
    public string Remarks { get; set; }
    public string InsertedBy { get; set; }
    public DateTime InsertedDate { get; set; }
    public string UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}