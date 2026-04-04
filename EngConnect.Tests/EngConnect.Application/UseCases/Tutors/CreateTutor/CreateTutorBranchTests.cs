using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.CreateTutor;

public class CreateTutorBranchTests
{
    [Theory]
    [MemberData(nameof(CreateTutorTestData.HandlerBranchCases), MemberType = typeof(CreateTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateTutorTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateTutorTestData.ValidatorBranchCases), MemberType = typeof(CreateTutorTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateTutorTestData.Definition, caseSet);
    }
}