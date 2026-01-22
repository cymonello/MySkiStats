namespace MySkiStats.Api.Models;

public class AthleteGear
{
    public int Id { get; set; }
    public int AthleteId { get; set; }
    public int GearTypeId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime BuyDate { get; set; }
    
    public Athlete Athlete { get; set; } = null!;
    public GearType GearType { get; set; } = null!;
    public ICollection<Activity> Activities { get; set; } = [];
}