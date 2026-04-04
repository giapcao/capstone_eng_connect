using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

public class CreateLessonRescheduleRequestBranchTests
{
    [Theory]
    [MemberData(nameof(CreateLessonRescheduleRequestTestData.HandlerBranchCases), MemberType = typeof(CreateLessonRescheduleRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonRescheduleRequestTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateLessonRescheduleRequestTestData.ValidatorBranchCases), MemberType = typeof(CreateLessonRescheduleRequestTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonRescheduleRequestTestData.Definition, caseSet);
    }
}