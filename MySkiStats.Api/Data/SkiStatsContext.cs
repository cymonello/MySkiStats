using Microsoft.EntityFrameworkCore;
using MySkiStats.Api.Models;

namespace MySkiStats.Api.Data;

public class SkiStatsContext : DbContext
{
    public SkiStatsContext(DbContextOptions<SkiStatsContext> options) : base(options) { }
    
    public DbSet<Athlete> Athletes { get; set; } = null!;
    public DbSet<ActivityType> ActivityTypes { get; set; } = null!;
    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<GearType> GearTypes { get; set; } = null!;
    public DbSet<AthleteGear> AthleteGears { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Activity relationships
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Athlete)
            .WithMany(a => a.Activities)
            .HasForeignKey(a => a.AthleteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.ActivityType)
            .WithMany(at => at.Activities)
            .HasForeignKey(a => a.ActivityTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.AthleteGear)
            .WithMany(ag => ag.Activities)
            .HasForeignKey(a => a.AthleteGearId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // AthleteGear relationships
        modelBuilder.Entity<AthleteGear>()
            .HasOne(ag => ag.Athlete)
            .WithMany(a => a.Gears)
            .HasForeignKey(ag => ag.AthleteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<AthleteGear>()
            .HasOne(ag => ag.GearType)
            .WithMany(gt => gt.Gears)
            .HasForeignKey(ag => ag.GearTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}