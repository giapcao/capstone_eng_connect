using EngConnect.Application.UseCases.CourseVerificationRequests.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.GetCourseVerificationRequestById;

public record GetCourseVerificationRequestByIdQuery(Guid Id) : IQuery<GetCourseVerificationRequestResponse>;
