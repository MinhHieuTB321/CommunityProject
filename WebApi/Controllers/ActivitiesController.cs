using Application.GlobalExceptionHandling.Exceptions;
using Application.Services.Interfaces;
using Application.ViewModels.ActivityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ActivitiesController : BaseController
{
    private readonly IActivitiesService _activityService;

    public ActivitiesController(IActivitiesService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(Guid communityId)
    {
        var result = await _activityService.GetAllActivities(communityId);
        return Ok(result);
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _activityService.GetActivityById(id);
        return Ok(result);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateActivityModel model)
    {
        await _activityService.CreateAsync(model);
        return StatusCode(201);
    }
    /// <summary>
    /// Api để tạo like vs dislike
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id}/members")]
    [Authorize]
    public async Task<IActionResult> CreateLike(Guid id)
    {
        await _activityService.LikedAsync(id);
        return StatusCode(201);
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateActivityModel model)
    {
        if (id != model.Id) throw new BadRequestException("Id is not matching");
        await _activityService.UpdateAsync(model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _activityService.DeleteAsync(id);
        return NoContent();
    }
}