using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetTutorById;

public class GetTutorByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetTutorByIdTestData.NormalHandlerCases), MemberType = typeof(GetTutorByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorByIdTestData.Definition, caseSet);
    }
}