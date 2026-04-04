using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

public class UpdateLessonRescheduleRequestBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonRescheduleRequestTestData.HandlerBranchCases), MemberType = typeof(UpdateLessonRescheduleRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonRescheduleRequestTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateLessonRescheduleRequestTestData.ValidatorBranchCases), MemberType = typeof(UpdateLessonRescheduleRequestTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonRescheduleRequestTestData.Definition, caseSet);
    }
}