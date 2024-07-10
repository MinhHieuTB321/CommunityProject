using Application.ViewModels.AuthModels;
using Application.ViewModels.UserModels;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<AuthResponseModel> AuthenticateGoogleAsync(string token);
    Task<List<ViewUserModel>> GetAllAsync();
    Task<ViewUserModel> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task CreateAsync(CreateUserModel model);
    Task UpdateAsync(UpdateUserModel model);
}