using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

public class UpdateIntroVideoUrlTutorBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateIntroVideoUrlTutorTestData.HandlerBranchCases), MemberType = typeof(UpdateIntroVideoUrlTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateIntroVideoUrlTutorTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateIntroVideoUrlTutorTestData.ValidatorBranchCases), MemberType = typeof(UpdateIntroVideoUrlTutorTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateIntroVideoUrlTutorTestData.Definition, caseSet);
    }
}