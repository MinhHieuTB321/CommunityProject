using Application.ViewModels.CommunityModels;

namespace Application.Services.Interfaces;

public interface ICommunityService
{
    Task<List<ViewCommunityModel>> GetAllAsync();
    Task<ViewCommunityModel> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task CreateAsync(CreateCommunityModel model);
    Task UpdateAsync(UpdateCommunityModel model);
    Task JoinAsync(Guid communityId);
}