using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Quiz : AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsOpen { get; set; }

    public int MaxScore { get; set; }

    public int DurationSeconds { get; set; }

    public int? AttemptLimit { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
