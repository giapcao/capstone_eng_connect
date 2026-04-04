using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetListStudents;

public class GetListStudentsHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListStudentsTestData.NormalHandlerCases), MemberType = typeof(GetListStudentsTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListStudentsTestData.Definition, caseSet);
    }
}