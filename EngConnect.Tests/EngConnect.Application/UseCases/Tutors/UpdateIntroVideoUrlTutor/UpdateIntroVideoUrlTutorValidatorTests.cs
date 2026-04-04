using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

public class UpdateIntroVideoUrlTutorValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateIntroVideoUrlTutorTestData.NormalValidatorCases), MemberType = typeof(UpdateIntroVideoUrlTutorTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateIntroVideoUrlTutorTestData.Definition, caseSet);
    }
}