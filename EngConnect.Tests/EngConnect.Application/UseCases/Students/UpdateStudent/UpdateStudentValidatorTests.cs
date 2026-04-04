using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStudent;

public class UpdateStudentValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateStudentTestData.NormalValidatorCases), MemberType = typeof(UpdateStudentTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateStudentTestData.Definition, caseSet);
    }
}