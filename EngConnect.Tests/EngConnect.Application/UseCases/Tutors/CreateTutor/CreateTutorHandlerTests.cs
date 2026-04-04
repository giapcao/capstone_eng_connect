using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.CreateTutor;

public class CreateTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateTutorTestData.NormalHandlerCases), MemberType = typeof(CreateTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateTutorTestData.Definition, caseSet);
    }
}