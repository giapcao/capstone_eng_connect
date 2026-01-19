using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class LessonScript : AuditableEntity<Guid>
{
    public Guid LessonId { get; set; }

    public Guid RecordId { get; set; }

    public string? Language { get; set; }

    public string? FullText { get; set; }

    public string? SummarizeText { get; set; }

    public string? LessonOutcome { get; set; }

    public decimal? CoveragePercent { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual LessonRecord Record { get; set; } = null!;
}
