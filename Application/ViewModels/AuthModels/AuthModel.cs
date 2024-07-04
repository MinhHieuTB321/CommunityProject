using System.ComponentModel.DataAnnotations;
using Application.ViewModels.UserModels;

namespace Application.ViewModels.AuthModels;

public class AuthModel
{
    // [Required]
    // public string GoogleToken { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class AuthResponseModel
{
    public string AccessToken { get; set; } = default!;
    public ViewUserModel? User { get; set; }
}