using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetCvUrlTutor;

public class GetCvUrlTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(GetCvUrlTutorTestData.NormalHandlerCases), MemberType = typeof(GetCvUrlTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetCvUrlTutorTestData.Definition, caseSet);
    }
}