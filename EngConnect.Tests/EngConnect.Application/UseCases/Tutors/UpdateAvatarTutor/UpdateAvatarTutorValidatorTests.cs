using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;

public class UpdateAvatarTutorValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateAvatarTutorTestData.NormalValidatorCases), MemberType = typeof(UpdateAvatarTutorTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateAvatarTutorTestData.Definition, caseSet);
    }
}