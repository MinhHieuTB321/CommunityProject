using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.UserModels;

public class CreateUserModel
{
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
    [Required]
    public string ConfirmPassword { get; set; } = default!;
}

public class UpdateUserModel : CreateUserModel
{
    [Required]
    public Guid Id { get; set; }
}

public class ViewUserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime ModificationDate { get; set; }
    public DateTime CreationDate { get; set; }

}