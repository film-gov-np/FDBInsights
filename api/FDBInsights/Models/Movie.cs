namespace FDBInsights.Models;

public class Movies
{
    public int MovieID { get; set; }
    public string MovieCode { get; set; } = string.Empty;
    public string ProductionHouseCode { get; set; } = string.Empty;
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}