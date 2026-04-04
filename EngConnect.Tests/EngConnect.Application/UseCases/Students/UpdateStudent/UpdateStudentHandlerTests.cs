using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStudent;

public class UpdateStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateStudentTestData.NormalHandlerCases), MemberType = typeof(UpdateStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateStudentTestData.Definition, caseSet);
    }
}