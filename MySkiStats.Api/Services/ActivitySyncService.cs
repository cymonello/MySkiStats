using Microsoft.EntityFrameworkCore;
using MySkiStats.Api.Data;
using MySkiStats.Api.Models;

namespace MySkiStats.Api.Services;

public class ActivitySyncService
{
    private readonly SkiStatsContext _context;
    private readonly StravaService _stravaService;

    public ActivitySyncService(SkiStatsContext context, StravaService stravaService)
    {
        _context = context;
        _stravaService = stravaService;
    }

    public async Task<Athlete?> SyncAthleteActivitiesAsync(string accessToken)
    {
        // Get athlete info from Strava
        var stravaAthlete = await _stravaService.GetAthleteAsync(accessToken);
        if (stravaAthlete == null)
            return null;

        // Check if athlete already exists in database
        var athlete = await _context.Athletes.FirstOrDefaultAsync(a => a.StravaId == stravaAthlete.Id);
        
        if (athlete == null)
        {
            // New athlete - create record
            athlete = new Athlete { StravaId = stravaAthlete.Id };
            _context.Athletes.Add(athlete);
            await _context.SaveChangesAsync();
            athlete = await _context.Athletes.FirstOrDefaultAsync(a => a.StravaId == stravaAthlete.Id);
            // Fetch and store activities from Strava
            await FetchAndStoreActivitiesAsync(athlete, accessToken);
        }
        else
        {
            // Athlete exists - optionally refresh activities
            await FetchAndStoreActivitiesAsync(athlete, accessToken);
        }

        return athlete;
    }

    private async Task FetchAndStoreActivitiesAsync(Athlete athlete, string accessToken)
    {
        var stravaActivities = await _stravaService.GetActivitiesAsync(accessToken);
        if (stravaActivities == null)
            return;

        // Get or create "Backcountry Skiing" activity type
        var activityType = await _context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "BackcountrySki")
            ?? new ActivityType { Name = "BackcountrySki" };

        if (activityType.Id == 0)
            _context.ActivityTypes.Add(activityType);

        await _context.SaveChangesAsync();

        foreach (var stravaActivity in stravaActivities)
        {
            // Check if activity already exists
            var existingActivity = await _context.Activities.FirstOrDefaultAsync(a => a.StravaId == stravaActivity.Id);
            if (existingActivity != null)
                continue;

            if (stravaActivity.Type != "BackcountrySki")
                continue;

            var activity = new Activity
            {
                AthleteId = athlete.Id,
                ActivityTypeId = activityType.Id,
                Name = stravaActivity.Name,
                Distance = stravaActivity.Distance,
                Ascent = stravaActivity.Elevation,
                //Descent = stravaActivity.TotalElevationLoss,
                MovingTime = stravaActivity.MovingTime,
                ElapsedTime = stravaActivity.ElapsedTime,
                Date = stravaActivity.StartDate,
                StravaId = stravaActivity.Id
            };

            _context.Activities.Add(activity);
        }

        await _context.SaveChangesAsync();
    }
}