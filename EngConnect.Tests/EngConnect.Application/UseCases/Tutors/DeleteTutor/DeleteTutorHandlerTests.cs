using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.DeleteTutor;

public class DeleteTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorTestData.NormalHandlerCases), MemberType = typeof(DeleteTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteTutorTestData.Definition, caseSet);
    }
}