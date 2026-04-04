using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetListTutor;

public class GetListTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListTutorTestData.NormalHandlerCases), MemberType = typeof(GetListTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorTestData.Definition, caseSet);
    }
}