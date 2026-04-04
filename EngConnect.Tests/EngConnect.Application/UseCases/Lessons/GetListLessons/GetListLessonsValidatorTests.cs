using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetListLessons;

public class GetListLessonsValidatorTests
{
    [Theory]
    [MemberData(nameof(GetListLessonsTestData.NormalValidatorCases), MemberType = typeof(GetListLessonsTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListLessonsTestData.Definition, caseSet);
    }
}