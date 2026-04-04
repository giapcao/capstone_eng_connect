using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonScripts.GenerateQuizByLessonScriptId;

public class GenerateQuizByLessonScriptIdQueryHandler : IQueryHandler<GenerateQuizByLessonScriptIdQuery, GenerateQuizResponse>
{
    private readonly ILogger<GenerateQuizByLessonScriptIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiService _aiService;

    public GenerateQuizByLessonScriptIdQueryHandler(
        IUnitOfWork unitOfWork, 
        ILogger<GenerateQuizByLessonScriptIdQueryHandler> logger,
        IAiService aiService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _aiService = aiService;
    }

    public async Task<Result<GenerateQuizResponse>> HandleAsync(
        GenerateQuizByLessonScriptIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GenerateQuizByLessonScriptIdQueryHandler: {@query}", query);
        try
        {
            var lessonScript = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .FindByIdAsync(query.LessonScriptId, false, cancellationToken: cancellationToken);

            if (lessonScript == null)
            {
                _logger.LogWarning("LessonScript không tồn tại {LessonScriptId}", query.LessonScriptId);
                return Result.Failure<GenerateQuizResponse>(HttpStatusCode.BadRequest,
                    CommonErrors.NotFound<LessonScript>("Id"));
            }
            
            if (ValidationUtil.IsNullOrEmpty(lessonScript.SummarizeText))
            {
                _logger.LogWarning("LessonScript không có SummarizeText {LessonScriptId}", query.LessonScriptId);
                return Result.Failure<GenerateQuizResponse>(HttpStatusCode.BadRequest,
                    new Error("EmptySummarizeText", "LessonScript không có nội dung tóm tắt để tạo quiz"));
            }
            
            _logger.LogInformation("Calling AI Service to generate quiz from LessonScriptId: {LessonScriptId}", query.LessonScriptId);
            var quizResponse = await _aiService.GenerateQuizAsync(
                new GenerateQuizRequest(lessonScript.SummarizeText));

            if (quizResponse == null)
            {
                _logger.LogWarning("AI Service failed to generate quiz for LessonScriptId: {LessonScriptId}", query.LessonScriptId);
                return Result.Failure<GenerateQuizResponse>(HttpStatusCode.BadRequest,
                    new Error("GenerateQuizFailed", "Không thể tạo quiz. Vui lòng thử lại"));
            }

            _logger.LogInformation(
                "End GenerateQuizByLessonScriptIdQueryHandler successfully. Generated {QuestionCount} questions",
                quizResponse.Questions.Count);
            return Result.Success(quizResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GenerateQuizByLessonScriptIdQueryHandler: {@Message}", ex.Message);
            return Result.Failure<GenerateQuizResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
    public static string CleanAiJson(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "[]";
        int start = input.IndexOf('[');
        int end = input.LastIndexOf(']');

        if (start != -1 && end != -1 && end > start)
        {
            return input.Substring(start, (end - start) + 1);
        }

        return input;
    }
}