namespace MySkiStats.Api.Models;

public class Activity
{
    public int Id { get; set; }
    public long StravaId { get; set; }
    public int ActivityTypeId { get; set; }
    public int AthleteId { get; set; }
    public DateTime Date { get; set; }
    public double Distance { get; set; }
    public int Ascent { get; set; }
    public int Descent { get; set; }
    public double Grade { get; set; }
    public double ElapsedTime { get; set; }
    public double MovingTime { get; set; }
    public int? AthleteGearId { get; set; }
    public string? Comments { get; set; }
    public int Runs { get; set; }
    
    public ActivityType ActivityType { get; set; } = null!;
    public Athlete Athlete { get; set; } = null!;
    public AthleteGear? AthleteGear { get; set; }
}