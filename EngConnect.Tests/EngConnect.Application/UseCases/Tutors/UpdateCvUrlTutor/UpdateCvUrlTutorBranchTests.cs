using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;

public class UpdateCvUrlTutorBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateCvUrlTutorTestData.HandlerBranchCases), MemberType = typeof(UpdateCvUrlTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateCvUrlTutorTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateCvUrlTutorTestData.ValidatorBranchCases), MemberType = typeof(UpdateCvUrlTutorTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateCvUrlTutorTestData.Definition, caseSet);
    }
}