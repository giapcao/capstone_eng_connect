using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetListStudents;

public class GetListStudentsBranchTests
{
    [Theory]
    [MemberData(nameof(GetListStudentsTestData.HandlerBranchCases), MemberType = typeof(GetListStudentsTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListStudentsTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetListStudentsTestData.ValidatorBranchCases), MemberType = typeof(GetListStudentsTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListStudentsTestData.Definition, caseSet);
    }
}