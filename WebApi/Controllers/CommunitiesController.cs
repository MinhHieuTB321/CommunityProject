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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await _communityService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCommunityModel model)
    {
        await _communityService.CreateAsync(model);
        return StatusCode(201);
    }

    /// <summary>
    /// Api để tham gia vào Community
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

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCommunityModel model)
    {
        if (id != model.Id) throw new BadRequestException("Id is not matching");
        await _communityService.UpdateAsync(model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _communityService.DeleteAsync(id);
        return NoContent();
    }
}