using Application.ViewModels.ActivityModel;

namespace Application.Services.Interfaces;

public interface IActivitiesService
{
    Task<List<ViewActivityModel>> GetAllActivities(Guid comId);
    Task<ViewActivityModel> GetActivityById(Guid id);
    Task<bool> DeleteAsync(Guid id);

    Task CreateAsync(CreateActivityModel model);
    Task UpdateAsync(UpdateActivityModel model);

    Task LikedAsync(Guid activityId);
}