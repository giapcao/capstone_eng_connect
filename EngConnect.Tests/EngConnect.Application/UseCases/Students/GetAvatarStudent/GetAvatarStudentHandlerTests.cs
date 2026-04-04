using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetAvatarStudent;

public class GetAvatarStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(GetAvatarStudentTestData.NormalHandlerCases), MemberType = typeof(GetAvatarStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetAvatarStudentTestData.Definition, caseSet);
    }
}