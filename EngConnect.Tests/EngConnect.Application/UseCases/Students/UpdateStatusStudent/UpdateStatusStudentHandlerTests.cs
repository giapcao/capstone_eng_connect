using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStatusStudent;

public class UpdateStatusStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateStatusStudentTestData.NormalHandlerCases), MemberType = typeof(UpdateStatusStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateStatusStudentTestData.Definition, caseSet);
    }
}