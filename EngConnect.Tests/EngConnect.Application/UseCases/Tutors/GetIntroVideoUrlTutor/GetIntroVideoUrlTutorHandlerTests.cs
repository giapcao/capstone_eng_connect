using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;

public class GetIntroVideoUrlTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(GetIntroVideoUrlTutorTestData.NormalHandlerCases), MemberType = typeof(GetIntroVideoUrlTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetIntroVideoUrlTutorTestData.Definition, caseSet);
    }
}