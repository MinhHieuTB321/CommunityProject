using System.Diagnostics.CodeAnalysis;
using Application.ViewModels.UserModels;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.CommunityModels;

public class CreateCommunityModel
{
    public string GroupName { get; set; } = default!;
    public string GroupGoal { get; set; } = default!;
    public string HabitType { get; set; } = default!;
    public int Period { get; set; }
    [AllowNull]
    public IFormFile? Image { get; set; } = default!;
}
public class UpdateCommunityModel : CreateCommunityModel
{
    public Guid Id { get; set; }
}

public class ViewCommunityModel
{
    public Guid Id { get; set; }
    public string GroupName { get; set; } = default!;
    public string GroupGoal { get; set; } = default!;
    public string HabitType { get; set; } = default!;
    public int Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ImageName { get; set; }
    public string? ImageUrl { get; set; }
    public int NumOfMember { get; set; }
    public bool IsJoined { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreationDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModificationDate { get; set; } = null;
    // public List<ViewUserModel>? Members { get; set; }
}

public class CreateMemberModel
{
    public Guid CommunityID { get; set; }
    public Guid UserID { get; set; }
}
