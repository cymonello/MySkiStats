using Microsoft.AspNetCore.Mvc;
using MySkiStats.Api.Data;
using MySkiStats.Api.Services;
using Microsoft.EntityFrameworkCore;
using MySkiStats.Api.Dto;

namespace MySkiStats.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AthletesController : ControllerBase
{
    private readonly ActivitySyncService _syncService;
    private readonly SkiStatsContext _context;

    public AthletesController(ActivitySyncService syncService, SkiStatsContext context)
    {
        _syncService = syncService;
        _context = context;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncActivities([FromBody] SyncRequest request)
    {
        var athlete = await _syncService.SyncAthleteActivitiesAsync(request.AccessToken);
        if (athlete == null)
            return Unauthorized("Failed to authenticate with Strava");

        var activities = await _context.Activities
            .Where(a => a.AthleteId == athlete.Id)
            .OrderByDescending(a => a.Date)
            .Select(a => new ActivityDto
            {
                Id = a.Id,
                StravaId = a.StravaId,
                AthleteId = a.AthleteId,
                Date = a.Date,
                Name = a.Name,
                Distance = a.Distance,
                Ascent = a.Ascent,
                Descent = a.Descent,
                Grade = a.Grade,
                ElapsedTime = a.ElapsedTime,
                MovingTime = a.MovingTime,
                AthleteGearId = a.AthleteGearId,
                Comments = a.Comments,
                Runs = a.Runs,
                ActivityTypeName = a.ActivityType.Name
            })
            .ToListAsync();

        return Ok(new { athlete = athlete.ToDto(), activities = activities });
    }

    public class SyncRequest
    {
        public string AccessToken { get; set; } = default!;
    }
}