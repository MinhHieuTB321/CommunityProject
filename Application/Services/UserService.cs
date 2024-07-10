using Application.GlobalExceptionHandling.Exceptions;
using Application.Services.Interfaces;
using Application.Utilities;
using Application.ViewModels.AuthModels;
using Application.ViewModels.UserModels;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Firebase.Auth;
using User = Domain.Entities.User;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IMongoRepository _repository;
    private readonly IMapper _mapper;
    private readonly string _tableName;
    private readonly AppSettings _appSettings;
    public UserService(IMongoRepository repository, IMapper mapper, AppSettings appSettings)
    {
        _appSettings = appSettings;
        _repository = repository;
        _mapper = mapper;
        _tableName = TableEnums.Users.ToString();
    }


    public async Task<AuthResponseModel> AuthenticateGoogleAsync(string token)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
        var userFirebase = await auth.GetUserAsync(token)
            ?? throw new BadRequestException("Token is not valid!");
        if (userFirebase is null)
            throw new Exception($"Error at: {nameof(UserService)}_ User not exist on firebase authentication");
        var allUsers = await _repository.GetAllAsync<User>(_tableName);
        var tmp = allUsers.FirstOrDefault(u => u.Email == userFirebase.Email);
        var user = tmp is not null ?
        tmp
        :
        new User
        {
            Name = userFirebase.DisplayName,
            Email = userFirebase.Email,
            Password = userFirebase.Email.Split('@')[0],
        };
        if (user is null)
            await _repository.InsertAsync<User>(_tableName, user!);
        return new AuthResponseModel
        {
            AccessToken = user!.GenerateJsonWebToken(_appSettings),
            User = _mapper.Map<ViewUserModel>(user)
        };
    }

    public async Task CreateAsync(CreateUserModel model)
    {
        try
        {
            if (!model.Password.Equals(model.ConfirmPassword))
                throw new BadRequestException("Password is not match!");
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
            var resultFire = await auth.CreateUserWithEmailAndPasswordAsync(email: model.Email, password: model.Password);
            if (resultFire.User != null)
            {
                var record = _mapper.Map<User>(model);
                await _repository.InsertAsync<User>(_tableName, record);
            }
            else
            {
                throw new BadRequestException("Create user in firebase failed!");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error at: {nameof(UserService)}_ {ex.Message}");
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync<User>(_tableName, id);
        if (user == null)
            throw new NotFoundException("User is not exist!");
        return await _repository.DeleteAsync<User>(_tableName, id);
    }

    public async Task<List<ViewUserModel>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync<User>(_tableName);
        if (users is null)
            throw new BadRequestException("No users found in the repository");
        return _mapper.Map<List<ViewUserModel>>(users);
    }

    public async Task<ViewUserModel> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync<User>(_tableName, id);
        if (user == null)
            throw new NotFoundException("User is not exist!");
        return _mapper.Map<ViewUserModel>(user);
    }

    public async Task UpdateAsync(UpdateUserModel model)
    {
        var user = await _repository.GetByIdAsync<User>(_tableName, model.Id);
        if (user == null)
            throw new NotFoundException("User is not exist!");
        if (!model.Password.Equals(model.ConfirmPassword))
            throw new BadRequestException("Password is not match");
        var record = _mapper.Map(model, user);
        await _repository.UpsertAsync<User>(_tableName, model.Id, record);
    }
}
