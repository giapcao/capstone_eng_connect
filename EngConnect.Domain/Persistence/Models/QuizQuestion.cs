using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class QuizQuestion : AuditableEntity<Guid>
{
    public Guid QuizId { get; set; }

    public string QuestionType { get; set; } = null!;

    public int QuestionNumber { get; set; }

    public string QuestionText { get; set; } = null!;

    public string? Options { get; set; }

    public string? CorrectAnswer { get; set; }

    public int Point { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;
}
