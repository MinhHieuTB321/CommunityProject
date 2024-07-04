using Application.GlobalExceptionHandling.Exceptions;
using Application.Services.Interfaces;
using Application.ViewModels.CommunityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CommunitiesController : BaseController
{
    private readonly ICommunityService _communityService;
    public CommunitiesController(ICommunityService communityService)
    {
        _communityService = communityService;
    }
    /// <summary>
    /// Api use to get all Commnunity
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await _communityService.GetAllAsync();
        return Ok(result);
    }
    /// <summary>
    /// Api use to create Commnunity. Image is optional
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCommunityModel model)
    {
        await _communityService.CreateAsync(model);
        return StatusCode(201);
    }

    /// <summary>
    /// Api use to join Community
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id}/members")]
    [Authorize]
    public async Task<IActionResult> CreateMember(Guid id)
    {
        await _communityService.JoinAsync(id);
        return StatusCode(201);
    }
    /// <summary>
    /// Api use to update Community infor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCommunityModel model)
    {
        if (id != model.Id) throw new BadRequestException("Id is not matching");
        await _communityService.UpdateAsync(model);
        return NoContent();
    }
    /// <summary>
    /// Api use to delete Community by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _communityService.DeleteAsync(id);
        return NoContent();
    }
}