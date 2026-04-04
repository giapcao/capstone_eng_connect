using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

public class UpdateAvatarStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateAvatarStudentTestData.NormalHandlerCases), MemberType = typeof(UpdateAvatarStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateAvatarStudentTestData.Definition, caseSet);
    }
}