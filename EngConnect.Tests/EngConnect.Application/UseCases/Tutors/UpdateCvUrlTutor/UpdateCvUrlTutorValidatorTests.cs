using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;

public class UpdateCvUrlTutorValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateCvUrlTutorTestData.NormalValidatorCases), MemberType = typeof(UpdateCvUrlTutorTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateCvUrlTutorTestData.Definition, caseSet);
    }
}