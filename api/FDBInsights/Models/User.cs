namespace FDBInsights.Models;

public class User
{
    public int UserID { get; private set; }
    public string UserCode { get; private set; }
    public string FullName { get; set; }
    public string Email { get; private set; }
    public string UserName { get; set; }
    public string Password { get; private set; }
    public string Address { get; }
    public string Phone { get; }
    public string Mobile { get; }
    public string RoleID { get; private set; }
    public string Image { get; }
    public DateTime? ExpiryDate { get; }
    public int? StatusValue { get; }

    public bool IsActive { get; }
    // public int? DepartmentID { get; }
    // public int? DesignationID { get; }
    // public int? CountryID { get; }
    // public int? StateID { get; }
    // public int? CityID { get; }
    // public string AddedBy { get; }
    // public DateTime? AddedOn { get; }
    // public string UpdatedBy { get; }
    // public DateTime? UpdatedOn { get; }
    // public bool? IsDeleted { get; }
    // public bool? IsSuperUser { get; }
    // public bool? IsAllPrivilegeGranted { get; }
    // public DateTime? InactiveDateTime { get; }
}