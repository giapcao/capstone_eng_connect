using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.DeleteStudent;

public class DeleteStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteStudentTestData.NormalHandlerCases), MemberType = typeof(DeleteStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteStudentTestData.Definition, caseSet);
    }
}