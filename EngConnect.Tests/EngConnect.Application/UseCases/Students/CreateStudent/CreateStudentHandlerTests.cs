using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.CreateStudent;

public class CreateStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateStudentTestData.NormalHandlerCases), MemberType = typeof(CreateStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateStudentTestData.Definition, caseSet);
    }
}