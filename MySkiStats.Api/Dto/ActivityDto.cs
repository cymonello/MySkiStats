namespace MySkiStats.Api.Dto;

public class ActivityDto
{
    public int Id { get; set; }
    public long StravaId { get; set; }
    public int AthleteId { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; } = default!;
    public double Distance { get; set; }
    public double Ascent { get; set; }
    public double Descent { get; set; }
    public double Grade { get; set; }
    public double ElapsedTime { get; set; }
    public double MovingTime { get; set; }
    public int? AthleteGearId { get; set; }
    public string? Comments { get; set; }
    public int Runs { get; set; }
    public string ActivityTypeName { get; set; } = default!;
}
