using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

public class GetMeetingInfoQueryHandler : IQueryHandler<GetMeetingInfoQuery, GetMeetingInfoResponse>
{
    private readonly ILogger<GetMeetingInfoQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetMeetingInfoQueryHandler(ILogger<GetMeetingInfoQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetMeetingInfoResponse>> HandleAsync(
        GetMeetingInfoQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetMeetingInfoQueryHandler: {@Query}", query);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(query.LessonId, tracking: false, cancellationToken);

            if (lesson == null)
            {
                return Result.Failure<GetMeetingInfoResponse>(HttpStatusCode.NotFound,
                    MeetingErrors.LessonNotFound(query.LessonId));
            }

            // Verify user is part of this lesson
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var studentRepo = _unitOfWork.GetRepository<Student, Guid>();
            var isTutor = await tutorRepo.AnyAsync(t => t.Id == lesson.TutorId && t.UserId == query.UserId, cancellationToken);
            var isStudent = await studentRepo.AnyAsync(s => s.Id == lesson.StudentId && s.UserId == query.UserId, cancellationToken);

            if (!isTutor && !isStudent)
            {
                return Result.Failure<GetMeetingInfoResponse>(HttpStatusCode.Forbidden,
                    MeetingErrors.NotAuthorized());
            }

            var participantRepo = _unitOfWork.GetRepository<MeetingParticipant, Guid>();
            var participants = await participantRepo
                .FindAll(p => p.LessonId == query.LessonId, tracking: false, cancellationToken)
                .Select(p => new ParticipantInfo
                {
                    UserId = p.UserId,
                    Role = p.Role,
                    JoinedAt = p.JoinedAt,
                    LeftAt = p.LeftAt
                })
                .ToListAsync(cancellationToken);

            var response = new GetMeetingInfoResponse
            {
                LessonId = lesson.Id,
                RoomId = lesson.MeetingUrl,
                MeetingStatus = lesson.MeetingStatus,
                MeetingStartedAt = lesson.MeetingStartedAt,
                MeetingEndedAt = lesson.MeetingEndedAt,
                Participants = participants
            };

            _logger.LogInformation("End GetMeetingInfoQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetMeetingInfoQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetMeetingInfoResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}