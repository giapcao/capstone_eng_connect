using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetStudentById;

public class GetStudentByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetStudentByIdTestData.NormalHandlerCases), MemberType = typeof(GetStudentByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetStudentByIdTestData.Definition, caseSet);
    }
}