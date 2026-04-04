using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;

public class UpdateCvUrlTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateCvUrlTutorTestData.NormalHandlerCases), MemberType = typeof(UpdateCvUrlTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateCvUrlTutorTestData.Definition, caseSet);
    }
}