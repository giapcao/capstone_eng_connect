using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.CreateStudent;

public class CreateStudentValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateStudentTestData.NormalValidatorCases), MemberType = typeof(CreateStudentTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateStudentTestData.Definition, caseSet);
    }
}