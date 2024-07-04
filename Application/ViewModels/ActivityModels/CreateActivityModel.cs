using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.ActivityModel;

public class CreateActivityModel
{
    public Guid CommunityId { get; set; }
    public IFormFile ImageFile { get; set; } = default!;
    public string Description { get; set; } = default!;
}
public class UpdateActivityModel
{
    public Guid Id { get; set; }
    [AllowNull]
    public IFormFile? ImageFile { get; set; } = default!;
    public string Description { get; set; } = default!;
}

public class ViewActivityModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CommunityId { get; set; }
    public string? ImageName { get; set; }
    public string? ImageUrl { get; set; }
    public string Description { get; set; } = default!;
    public bool IsLiked { get; set; } = false;
    public List<Guid>? PostLikes { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? ModificationDate { get; set; } = null;

}