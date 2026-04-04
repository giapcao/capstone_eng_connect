using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetAvatarTutor;

public class GetAvatarTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(GetAvatarTutorTestData.NormalHandlerCases), MemberType = typeof(GetAvatarTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetAvatarTutorTestData.Definition, caseSet);
    }
}