using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateTutor;

public class UpdateTutorValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateTutorTestData.NormalValidatorCases), MemberType = typeof(UpdateTutorTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateTutorTestData.Definition, caseSet);
    }
}