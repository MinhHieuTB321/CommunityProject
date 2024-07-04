using Application.GlobalExceptionHandling.Exceptions;
using Application.Services.Interfaces;
using Application.ViewModels.CommunityModels;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Application.Services;

public class CommunityService : ICommunityService
{

    private readonly IMongoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claims;
    private readonly string _tableName;
    private readonly string _tableUser = TableEnums.Users.ToString();

    public CommunityService(IMongoRepository repository, IMapper mapper, IClaimsService claims)
    {
        _claims = claims;
        _repository = repository;
        _mapper = mapper;
        _tableName = TableEnums.Communities.ToString();
    }
    public async Task CreateAsync(CreateCommunityModel model)
    {
        var record = _mapper.Map<Community>(model);
        record.StartDate = DateTime.Now;
        record.EndDate = DateTime.Now.AddDays(model.Period);
        record.Members!.Add(_claims.GetCurrentUser);
        await _repository.InsertAsync<Community>(_tableName, record);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var community = await _repository.GetByIdAsync<Community>(_tableName, id);
        if (community == null)
            throw new BadRequestException("Community is not found!");
        if (community.CreatedBy != _claims.GetCurrentUser)
            throw new BadRequestException("You are not allowed to delete this community");
        return await _repository.DeleteAsync<Community>(_tableName, id);
    }

    public async Task<List<ViewCommunityModel>> GetAllAsync()
    {
        var communities = await _repository.GetAllAsync<Community>(_tableName);
        var result = _mapper.Map<List<ViewCommunityModel>>(communities);
        for (int i = 0; i < communities.Count; i++)
        {
            if (communities[i].Members!.Count > 0 && communities[i].Members!.Contains(_claims.GetCurrentUser))
            {
                result[i].IsJoined = true;
            }
        }
        return result;
    }
    public async Task<ViewCommunityModel> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync<User>(_tableName, id);
        if (user == null) throw new NotFoundException("User is not exist!");
        return _mapper.Map<ViewCommunityModel>(user);
    }

    public async Task JoinAsync(Guid communityId)
    {
        var community = await _repository.GetByIdAsync<Community>(_tableName, communityId);
        if (community.CreatedBy != _claims.GetCurrentUser)
            throw new BadRequestException("You are not allowed to update this community");
        if (community.NumOfMember > 10)
            throw new BadRequestException("Community is full");
        if (community.Members!.Contains(_claims.GetCurrentUser))
            throw new BadRequestException("You are already joined this community");
        community.Members!.Add(_claims.GetCurrentUser);
        await _repository.UpsertAsync<Community>(_tableName, communityId, community);
    }

    public async Task UpdateAsync(UpdateCommunityModel model)
    {
        var community = await _repository.GetByIdAsync<Community>(_tableName, model.Id);
        if (community.CreatedBy != _claims.GetCurrentUser)
            throw new BadRequestException("You are not allowed to update this community");
        var record = _mapper.Map(model, community);
        await _repository.UpsertAsync<Community>(_tableName, model.Id, record);
    }
}