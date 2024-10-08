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
    /// <summary>
    /// Api use to get all Task belong one Commnunity
    /// </summary>
    /// <param name="communityId"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(Guid communityId)
    {
        var result = await _activityService.GetAllActivities(communityId);
        return Ok(result);
    }
    /// <summary>
    /// Api use to Get Task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _activityService.GetActivityById(id);
        return Ok(result);
    }
    /// <summary>
    /// Api để tạo Task cho 1 community
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Api use to update task. Image is optional
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateActivityModel model)
    {
        if (id != model.Id) throw new BadRequestException("Id is not matching");
        await _activityService.UpdateAsync(model);
        return NoContent();
    }
    /// <summary>
    /// Api use to delete task by TaskId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _activityService.DeleteAsync(id);
        return NoContent();
    }
}