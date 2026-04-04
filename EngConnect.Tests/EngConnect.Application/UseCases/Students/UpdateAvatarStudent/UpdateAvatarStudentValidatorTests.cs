using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

public class UpdateAvatarStudentValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateAvatarStudentTestData.NormalValidatorCases), MemberType = typeof(UpdateAvatarStudentTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateAvatarStudentTestData.Definition, caseSet);
    }
}