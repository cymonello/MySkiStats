namespace MySkiStats.Api.Models;

public class Athlete
{
    public int Id { get; set; }
    public int StravaId { get; set; }
    
    public ICollection<AthleteGear> Gears { get; set; } = [];
    public ICollection<Activity> Activities { get; set; } = [];
}