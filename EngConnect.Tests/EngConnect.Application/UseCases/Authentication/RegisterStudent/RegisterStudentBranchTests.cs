using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterStudent;

public class RegisterStudentBranchTests
{
    [Theory]
    [MemberData(nameof(RegisterStudentTestData.HandlerBranchCases), MemberType = typeof(RegisterStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterStudentTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(RegisterStudentTestData.ValidatorBranchCases), MemberType = typeof(RegisterStudentTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterStudentTestData.Definition, caseSet);
    }
}