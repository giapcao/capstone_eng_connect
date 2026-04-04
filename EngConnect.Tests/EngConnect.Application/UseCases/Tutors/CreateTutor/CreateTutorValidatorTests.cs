using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.CreateTutor;

public class CreateTutorValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateTutorTestData.NormalValidatorCases), MemberType = typeof(CreateTutorTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateTutorTestData.Definition, caseSet);
    }
}