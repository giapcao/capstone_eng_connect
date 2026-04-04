using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;

public class UpdateAvatarTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateAvatarTutorTestData.NormalHandlerCases), MemberType = typeof(UpdateAvatarTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateAvatarTutorTestData.Definition, caseSet);
    }
}