using EngConnect.BuildingBlock.Contracts.Models.Quartz;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class ScheduleJobTrackingConfiguration: IEntityTypeConfiguration<ScheduleJobTracking>
{
    public void Configure(EntityTypeBuilder<ScheduleJobTracking> builder)
    {
        builder.ToTable("schedule_job_tracking");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.JobName)
            .HasColumnName("job_name")
            .IsRequired();

        builder.Property(e => e.ExecuteAt)
            .HasColumnName("execute_at");

        builder.Property(e => e.LastFireAt)
            .HasColumnName("last_fire_at");

        builder.Property(e => e.NextFireAt)
            .HasColumnName("next_fire_at");

        builder.Property(e => e.RunCount)
            .HasColumnName("run_count");

        builder.Property(e => e.IsExecuted)
            .HasColumnName("is_executed");

        builder.Property(e => e.LastFireFailedAt)
            .HasColumnName("last_fire_failed_at");

        builder.Property(e => e.LastFireSucceededAt)
            .HasColumnName("last_fire_succeeded_at");
        
        builder.Property(e => e.JobType)
            .HasColumnName("job_type")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted");

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");

        // Covert enum to string
        builder.Property(x => x.JobType).HasConversion(
            p => p.ToString(),
            p => Enum.Parse<JobType>(p)
        );
    }
}