using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetListStudents;

public class GetListStudentsValidatorTests
{
    [Theory]
    [MemberData(nameof(GetListStudentsTestData.NormalValidatorCases), MemberType = typeof(GetListStudentsTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListStudentsTestData.Definition, caseSet);
    }
}