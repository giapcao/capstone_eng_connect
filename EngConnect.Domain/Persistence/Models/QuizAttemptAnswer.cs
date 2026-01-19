using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class QuizAttemptAnswer : AuditableEntity<Guid>
{
    public Guid AttemptId { get; set; }

    public Guid QuestionId { get; set; }

    public string? Answer { get; set; }

    public bool? IsCorrect { get; set; }

    public decimal? ReceivePoint { get; set; }
}
