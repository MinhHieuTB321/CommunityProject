namespace Domain.Entities;

public class Activity : BaseEntity
{
    public Guid CommunityId { get; set; }
    public string? ImageName { get; set; }
    public string? ImageUrl { get; set; }
    public string Description { get; set; } = default!;
    public List<Guid>? PostLikes { get; set; } = new List<Guid>();
}