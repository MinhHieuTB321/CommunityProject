using Application.Services.Interfaces;
using Application.ViewModels.AuthModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace WebAPI.Controllers;

public class AuthsController : BaseController
{
    private readonly IUserService _userService;
    public AuthsController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Api login via Google Firebase
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Login(string token)
    {
        var result = await _userService.AuthenticateGoogleAsync(token);
        return Ok(result);
    }
    /// <summary>
    /// Api use to login by email and password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> LoginByEmailPass([FromBody] AuthModel model)
    {
        var result = await _userService.AuthenticateEmailPassAsync(model);
        return Ok(result);
    }
}