using Application.GlobalExceptionHandling.Exceptions;
using Application.Services.Interfaces;
using Application.Utilities;
using Application.ViewModels.ActivityModel;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Application.Services;

public class ActivityService : IActivitiesService
{
    private readonly IMongoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claims;
    private readonly string _tableName;
    private readonly AppSettings _appSettings;

    public ActivityService(IMongoRepository repository,
    IMapper mapper, IClaimsService claims, AppSettings appSettings)
    {
        _appSettings = appSettings;
        _repository = repository;
        _mapper = mapper;
        _claims = claims;
        _tableName = TableEnums.Activities.ToString();
    }
    public async Task CreateAsync(CreateActivityModel model)
    {
        var image = await model.ImageFile.UploadFileAsync("Activities", _appSettings);
        var record = _mapper.Map<Activity>(model);
        record.ImageName = image.FileName;
        record.ImageUrl = image.URL;
        await _repository.InsertAsync<Activity>(_tableName, record);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var activity = await _repository.GetByIdAsync<Activity>(_tableName, id);
        if (activity == null)
            throw new BadRequestException("Activity is not found!");
        if (activity.CreatedBy != _claims.GetCurrentUser)
            throw new BadRequestException("You are not allowed to delete this activity!");
        return await _repository.DeleteAsync<Activity>(_tableName, id);
    }

    public async Task<ViewActivityModel> GetActivityById(Guid id)
    {
        var activity = await _repository.GetByIdAsync<Activity>(_tableName, id);
        if (activity == null)
            throw new BadRequestException("Activity is not found!");
        return _mapper.Map<ViewActivityModel>(activity);
    }

    public async Task<List<ViewActivityModel>> GetAllActivities(Guid comId)
    {
        var all = await _repository.GetAllAsync<Activity>(_tableName);
        if (all.Count == 0)
            throw new BadRequestException("No activities found!");
        var activities = all.Where(x => x.CommunityId == comId).ToList();
        if (activities.Count == 0)
            throw new BadRequestException("No activities found!");
        var result = _mapper.Map<List<ViewActivityModel>>(activities);
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i].PostLikes!.Count > 0 && result[i].PostLikes!.Contains(_claims.GetCurrentUser))
                result[i].IsLiked = true;
        }
        return result;

    }

    public async Task LikedAsync(Guid activityId)
    {
        var activity = await _repository.GetByIdAsync<Activity>(_tableName, activityId);
        if (activity == null)
            throw new BadRequestException("Activity is not found!");
        if (!activity.PostLikes!.Contains(_claims.GetCurrentUser))
        {
            activity.PostLikes.Add(_claims.GetCurrentUser);
        }
        else
        {
            activity.PostLikes.Remove(_claims.GetCurrentUser);
        }
        await _repository.UpsertAsync<Activity>(_tableName, activityId, activity);
    }

    public async Task UpdateAsync(UpdateActivityModel model)
    {
        var activity = await _repository.GetByIdAsync<Activity>(_tableName, model.Id);
        if (activity == null)
            throw new BadRequestException("Activity is not found!");
        var record = _mapper.Map(model, activity);
        if (model.ImageFile != null)
        {
            var image = await model.ImageFile.UploadFileAsync("Activities", _appSettings);
            activity.ImageName = image.FileName;
            activity.ImageUrl = image.URL;
        }
        await _repository.UpsertAsync<Activity>(_tableName, model.Id, activity);
    }
}