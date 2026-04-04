using System.Net;
using System.Text.Json;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryCommandHandler : ICommandHandler<GetAiSummaryCommand, AnalysisResponse>
{
    private readonly IAiService _aiService;
    private readonly ILogger<GetAiSummaryCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;

    public GetAiSummaryCommandHandler(IAiService aiService, IRedisService redisService, IUnitOfWork unitOfWork, ILogger<GetAiSummaryCommandHandler> logger)
    {
        _aiService = aiService;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _redisService = redisService;
    }

    public async Task<Result<AnalysisResponse>> HandleAsync(GetAiSummaryCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetAiSummaryCommandHandler {@Command}", command);
        try
        {
            var chunk = await _redisService.SortedSetRangeAsync(command.LessonId.ToString());
            var transcript = string.Join(" ", chunk
                    .Select(m => 
                    {
                        using var doc = JsonDocument.Parse(m.ToString());
                        return doc.RootElement.GetProperty("text").GetString();
                    })
                    .Where(t => !string.IsNullOrWhiteSpace(t)) 
            );
            var lesson = await _unitOfWork.GetRepository<Lesson, Guid>().FindByIdAsync(command.LessonId,
                false, cancellationToken: cancellationToken, l => l.Student.User
                , l => l.Tutor.User,l=>l.LessonRecord!,l=>l.Session!);
            if (lesson == null )
            {
                return Result.Failure<AnalysisResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("LessonId"));
            }
            
            if (lesson.LessonRecord == null) return Result.Failure<AnalysisResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonRecord>("Record"));
            if (lesson.Session?.Outcomes == null) return Result.Failure<AnalysisResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<CourseSession>("Session outcomes"));
            
            var analysisResponse = await _aiService.AnalyzeContentAsync( new AnalysisRequest(transcript, lesson.Session.Outcomes));

            if (analysisResponse is null)
            {
                _logger.LogWarning("AI summarize returned null for query {@Command}", command);
                return Result.Failure<AnalysisResponse>(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("gg "));
            }
            
            var passCount = analysisResponse.Detail.Pass.Count;
            var total = passCount + analysisResponse.Detail.Fail.Count;
        
            analysisResponse.CoveragePercent = total == 0 
                ? 0 
                : Math.Round((decimal)passCount / total * 100, 2);

            if (analysisResponse.CoveragePercent < SummarizePromptConstant.PercentageEvaluation)
            {
                AddWarningToOutBox(lesson, analysisResponse);
            }
            
            var lessonScript = CreateLessonScript(lesson.Id, lesson.LessonRecord.Id,transcript, analysisResponse);

            _unitOfWork.GetRepository<LessonScript, Guid>().Add(lessonScript);
            await _unitOfWork.SaveChangesAsync();
            await _redisService.DeleteCacheAsync(command.LessonId.ToString());
            _logger.LogInformation("End GetAiSummaryCommandHandler");
            return Result.Success(analysisResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetAiSummaryCommandHandler: {Message}", ex.Message);
            return Result.Failure<AnalysisResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    private void AddWarningToOutBox(Lesson lesson, AnalysisResponse analysisResponse)
    {
        var student = lesson.Student.User;
        var tutor = lesson.Tutor.User;
        
        var baseEvent = WarningInvalidLessonEvent.Create(student.Id,student.FirstName + " " + student.LastName
            ,student.Email,tutor.FirstName + " "+ tutor.LastName,tutor.Email,lesson.StartTime,lesson.EndTime
            ,analysisResponse.CoveragePercent,analysisResponse.Detail);
                
        var notificationEvent = NotificationHelper.CreateNotification(baseEvent, 
            [student.Id, tutor.Id],[],nameof(Channel.Email));
                
        var outboxEvent = OutboxEvent.CreateOutboxEvent(nameof(Lesson), lesson.Id,notificationEvent);
        _unitOfWork.GetRepository<OutboxEvent, Guid>().Add(outboxEvent);
    }
    
    private static LessonScript CreateLessonScript(Guid lessonId, Guid recordId, string transcript, AnalysisResponse analysisResponse)
    {
        return new LessonScript
        {
            LessonId = lessonId,
            RecordId = recordId,
            Language = nameof(Language.Vi),
            FullText = transcript,
            SummarizeText = analysisResponse.AiSummarizeText,
            LessonOutcome = JsonSerializer.Serialize(analysisResponse.Detail),
            CoveragePercent = analysisResponse.CoveragePercent 
        };
    }
}
