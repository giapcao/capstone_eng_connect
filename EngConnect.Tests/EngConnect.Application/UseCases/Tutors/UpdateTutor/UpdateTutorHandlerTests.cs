using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateTutor;

public class UpdateTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateTutorTestData.NormalHandlerCases), MemberType = typeof(UpdateTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateTutorTestData.Definition, caseSet);
    }
}