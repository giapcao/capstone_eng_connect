using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

public class UpdateIntroVideoUrlTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateIntroVideoUrlTutorTestData.NormalHandlerCases), MemberType = typeof(UpdateIntroVideoUrlTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateIntroVideoUrlTutorTestData.Definition, caseSet);
    }
}