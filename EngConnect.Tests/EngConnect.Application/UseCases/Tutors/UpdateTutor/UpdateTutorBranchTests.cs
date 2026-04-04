using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateTutor;

public class UpdateTutorBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateTutorTestData.HandlerBranchCases), MemberType = typeof(UpdateTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateTutorTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateTutorTestData.ValidatorBranchCases), MemberType = typeof(UpdateTutorTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateTutorTestData.Definition, caseSet);
    }
}