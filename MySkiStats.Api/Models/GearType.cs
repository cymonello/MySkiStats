namespace MySkiStats.Api.Models;

public class GearType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<AthleteGear> Gears { get; set; } = [];
}