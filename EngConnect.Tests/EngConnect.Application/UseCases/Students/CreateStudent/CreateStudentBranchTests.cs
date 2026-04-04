using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.CreateStudent;

public class CreateStudentBranchTests
{
    [Theory]
    [MemberData(nameof(CreateStudentTestData.HandlerBranchCases), MemberType = typeof(CreateStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateStudentTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateStudentTestData.ValidatorBranchCases), MemberType = typeof(CreateStudentTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateStudentTestData.Definition, caseSet);
    }
}