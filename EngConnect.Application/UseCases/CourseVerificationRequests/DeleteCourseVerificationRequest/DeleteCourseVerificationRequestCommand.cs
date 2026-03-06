using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.DeleteCourseVerificationRequest;

public record DeleteCourseVerificationRequestCommand(Guid Id) : ICommand;
