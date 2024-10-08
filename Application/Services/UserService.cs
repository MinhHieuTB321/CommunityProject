using Application.GlobalExceptionHandling.Exceptions;
using Application.Services.Interfaces;
using Application.Utilities;
using Application.ViewModels.AuthModels;
using Application.ViewModels.UserModels;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
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

    public async Task<AuthResponseModel> AuthenticateEmailPassAsync(AuthModel model)
    {
        var allUsers = await _repository.GetAllAsync<User>(_tableName);
        var user = allUsers.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
        if (user is null)
            throw new NotFoundException("User is not found in Database");
        return new AuthResponseModel
        {
            AccessToken = user.GenerateJsonWebToken(_appSettings),
            User = _mapper.Map<ViewUserModel>(user)
        };
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
        if (tmp is null)
        {
            tmp = new User
            {
                Name = userFirebase.DisplayName,
                Email = userFirebase.Email,
                Password = userFirebase.Email.Split('@')[0],
            };
            await _repository.InsertAsync<User>(_tableName, tmp);
        }
        return new AuthResponseModel
        {
            AccessToken = tmp!.GenerateJsonWebToken(_appSettings),
            User = _mapper.Map<ViewUserModel>(tmp)
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
        await DeleteUserFirebase(user.Email);
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

    private async Task DeleteUserFirebase(string email)
    {
        var user = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
        await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.DeleteUserAsync(user.Uid);

    }

    public async Task UpdateAsync(UpdateUserModel model)
    {
        var user = await _repository.GetByIdAsync<User>(_tableName, model.Id);
        if (user == null)
            throw new NotFoundException("User is not exist!");
        if (!user.Email.Equals(model.Email))
            throw new BadRequestException("Email cannot be change!");
        if (!model.Password.Equals(model.ConfirmPassword))
            throw new BadRequestException("Password is not match");
        if (!model.Password.Equals(user.Password))
        {
            await DeleteUserFirebase(user.Email);
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
            var resultFire = await auth.CreateUserWithEmailAndPasswordAsync(email: model.Email, password: model.Password);
            if (resultFire.User == null)
                throw new BadRequestException("Error at Firebase Authentication");
        }
        var record = _mapper.Map(model, user);
        await _repository.UpsertAsync<User>(_tableName, model.Id, record);
    }
}
