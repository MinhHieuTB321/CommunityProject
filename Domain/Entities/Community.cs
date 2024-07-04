namespace Domain.Entities;

public class Community : BaseEntity
{
    public string GroupName { get; set; } = default!;
    public string GroupGoal { get; set; } = default!;
    public string HabitType { get; set; } = default!;
    public int Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ImageName { get; set; }
    public string? ImageUrl { get; set; }
    public int NumOfMember { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Guid>? Members { get; set; } = new List<Guid>();
}