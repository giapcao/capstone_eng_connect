using System.Reflection;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Abstraction;

namespace EngConnect.Tests.Common;

internal static class UseCaseTestDataFactory
{
    private static readonly Assembly ApplicationAssembly = typeof(global::EngConnect.Application.AssemblyReference).Assembly;

    public static UseCaseScenario CreateValidScenario<TRequest>() where TRequest : class
    {
        var request = CreateValidRequest<TRequest>();
        return new UseCaseScenario
        {
            Request = request,
            Overrides = CreateDefaultOverrides(typeof(TRequest), valid: true)
        };
    }

    public static UseCaseScenario CreateValidScenario(string requestTypeFullName)
    {
        return CreateScenario(ResolveApplicationType(requestTypeFullName), valid: true);
    }

    public static UseCaseScenario CreateValidScenario(
        string requestTypeFullName,
        Action<object>? mutateRequest = null,
        Action<UseCaseMockContext>? arrangeMocks = null)
    {
        return CreateScenario(ResolveApplicationType(requestTypeFullName), valid: true, mutateRequest, arrangeMocks);
    }

    public static UseCaseScenario CreateInvalidScenario(string requestTypeFullName)
    {
        return CreateScenario(ResolveApplicationType(requestTypeFullName), valid: false);
    }

    public static UseCaseScenario CreateInvalidScenario<TRequest>() where TRequest : class
    {
        var request = CreateInvalidRequest<TRequest>();
        return new UseCaseScenario
        {
            Request = request,
            Overrides = CreateDefaultOverrides(typeof(TRequest), valid: false)
        };
    }

    public static UseCaseScenario CreateBoundaryScenario(
        string requestTypeFullName,
        Action<object> mutateRequest,
        Action<UseCaseMockContext>? arrangeMocks = null)
    {
        return CreateScenario(ResolveApplicationType(requestTypeFullName), valid: true, mutateRequest, arrangeMocks);
    }

    public static UseCaseScenario CreateDuplicateScenario(string requestTypeFullName)
    {
        return CreateScenario(
            ResolveApplicationType(requestTypeFullName),
            valid: true,
            mutateRequest: ForceDuplicateCandidates,
            arrangeMocks: mocks => mocks.UseSeededUnitOfWork(seedData: true));
    }

    public static TRequest CreateValidRequest<TRequest>() where TRequest : class
    {
        var request = TestObjectFactory.CreateValue<TRequest>();
        NormalizeRequest(request, valid: true);
        return request;
    }

    public static TRequest CreateInvalidRequest<TRequest>() where TRequest : class
    {
        var request = TestObjectFactory.CreateValue<TRequest>();
        NormalizeRequest(request, valid: false);
        return request;
    }

    private static UseCaseScenario CreateScenario(
        Type requestType,
        bool valid,
        Action<object>? mutateRequest = null,
        Action<UseCaseMockContext>? arrangeMocks = null)
    {
        var request = TestObjectFactory.CreateValue(requestType)
                      ?? throw new InvalidOperationException($"Cannot create request for {requestType.FullName}.");

        NormalizeRequest(request, valid);
        mutateRequest?.Invoke(request);

        return new UseCaseScenario
        {
            Request = request,
            Overrides = CreateDefaultOverrides(requestType, valid),
            ArrangeMocks = arrangeMocks
        };
    }

    private static IReadOnlyDictionary<Type, object?> CreateDefaultOverrides(Type requestType, bool valid)
    {
        var overrides = new Dictionary<Type, object?>();

        overrides[typeof(IUnitOfWork)] = new InMemoryUnitOfWork(seedData: true);

        return overrides;
    }

    private static Type ResolveApplicationType(string typeFullName)
    {
        return ApplicationAssembly.GetType(typeFullName, throwOnError: true, ignoreCase: false)
               ?? throw new InvalidOperationException($"Cannot resolve application type {typeFullName}.");
    }

    private static void NormalizeRequest(object request, bool valid)
    {
        NormalizeObject(request, request.GetType().Name, valid, depth: 0);
    }

    private static void ForceDuplicateCandidates(object request)
    {
        ApplyDuplicateCandidates(request, request.GetType().Name, depth: 0);
    }

    private static void NormalizeObject(object target, string requestTypeName, bool valid, int depth, bool forceSeedDefaults = false)
    {
        if (depth > 3)
        {
            return;
        }

        var targetType = target.GetType();
        var isCreateLike = requestTypeName.StartsWith("Create", StringComparison.Ordinal)
                           || requestTypeName.StartsWith("Register", StringComparison.Ordinal)
                           || requestTypeName.StartsWith("Add", StringComparison.Ordinal)
                           || requestTypeName.StartsWith("Upload", StringComparison.Ordinal);

        foreach (var property in targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanWrite || property.GetIndexParameters().Length != 0)
            {
                continue;
            }

            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlyingType == typeof(string))
            {
                property.SetValue(target, GetStringValue(requestTypeName, property.Name, valid, isCreateLike, forceSeedDefaults));
                continue;
            }

            if (underlyingType == typeof(Guid))
            {
                property.SetValue(target, valid ? UseCaseSeedDefaults.KnownGuid : Guid.Empty);
                continue;
            }

            if (underlyingType == typeof(DateTime))
            {
                property.SetValue(target, GetDateTimeValue(requestTypeName, property.Name, valid));
                continue;
            }

            if (underlyingType == typeof(DateTimeOffset))
            {
                property.SetValue(target, new DateTimeOffset(GetDateTimeValue(requestTypeName, property.Name, valid)));
                continue;
            }

            if (underlyingType == typeof(TimeOnly))
            {
                property.SetValue(target, GetTimeOnlyValue(property.Name, valid));
                continue;
            }

            if (underlyingType == typeof(int))
            {
                property.SetValue(target, GetIntegerValue(property.Name, valid));
                continue;
            }

            if (underlyingType == typeof(long))
            {
                property.SetValue(target, (long)GetIntegerValue(property.Name, valid));
                continue;
            }

            if (propertyType == typeof(FileUpload))
            {
                property.SetValue(target, CreateFileUpload(requestTypeName, valid));
                continue;
            }

            if (propertyType == typeof(Microsoft.AspNetCore.Http.IFormFile))
            {
                property.SetValue(target, valid
                    ? TestObjectFactory.CreateFormFile("test.txt", "text/plain", "hello world")
                    : TestObjectFactory.CreateFormFile("invalid", "application/octet-stream", string.Empty));
                continue;
            }

            if (propertyType == typeof(Stream))
            {
                property.SetValue(target, new MemoryStream(System.Text.Encoding.UTF8.GetBytes("hello world")));
                continue;
            }

            if (ShouldRecurseInto(propertyType))
            {
                var currentValue = property.GetValue(target) ?? TestObjectFactory.CreateValue(propertyType);
                if (currentValue != null)
                {
                    NormalizeObject(currentValue, requestTypeName, valid, depth + 1, forceSeedDefaults);
                    property.SetValue(target, currentValue);
                }
            }
        }
    }

    private static void ApplyDuplicateCandidates(object target, string requestTypeName, int depth)
    {
        if (depth > 3)
        {
            return;
        }

        var targetType = target.GetType();

        foreach (var property in targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanWrite || property.GetIndexParameters().Length != 0)
            {
                continue;
            }

            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlyingType == typeof(string))
            {
                var duplicateValue = GetDuplicateStringValue(requestTypeName, property.Name);
                if (duplicateValue != null)
                {
                    property.SetValue(target, duplicateValue);
                }

                continue;
            }

            if (ShouldRecurseInto(propertyType))
            {
                var currentValue = property.GetValue(target) ?? TestObjectFactory.CreateValue(propertyType);
                if (currentValue != null)
                {
                    ApplyDuplicateCandidates(currentValue, requestTypeName, depth + 1);
                    property.SetValue(target, currentValue);
                }
            }
        }
    }

    private static string GetStringValue(string requestTypeName, string propertyName, bool valid, bool isCreateLike, bool forceSeedDefaults)
    {
        if (forceSeedDefaults)
        {
            return GetSeedStringValue(propertyName);
        }

        if (propertyName.Contains("SortParams", StringComparison.OrdinalIgnoreCase))
        {
            return valid ? "createdat-desc" : string.Empty;
        }

        if (!valid)
        {
            if (propertyName.Contains("Email", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            if (propertyName.Contains("Phone", StringComparison.OrdinalIgnoreCase))
            {
                return "invalid-phone";
            }

            if (propertyName.Contains("Password", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            if (propertyName.Contains("Status", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Type", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Language", StringComparison.OrdinalIgnoreCase))
            {
                return "Invalid";
            }

            if (propertyName.Contains("FileName", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            if (propertyName.Contains("Name", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Title", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Subject", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Description", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("School", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Grade", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Class", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Notes", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Text", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Address", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Code", StringComparison.OrdinalIgnoreCase))
            {
                return new string('x', 600);
            }

            return string.Empty;
        }

        if (propertyName.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            return isCreateLike
                ? $"{requestTypeName.ToLowerInvariant()}@example.com"
                : UseCaseSeedDefaults.KnownEmail;
        }

        if (propertyName.Contains("Password", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownPassword;
        }

        if (propertyName.Contains("Phone", StringComparison.OrdinalIgnoreCase))
        {
            if (isCreateLike)
            {
                return requestTypeName.Contains("Register", StringComparison.OrdinalIgnoreCase)
                    ? "+84987654321"
                    : "0987654321";
            }

            return requestTypeName.Contains("RegisterUser", StringComparison.OrdinalIgnoreCase)
                || requestTypeName.Contains("ForgotPassword", StringComparison.OrdinalIgnoreCase)
                ? "+84912345678"
                : "0912345678";
        }

        if (propertyName.Contains("UserName", StringComparison.OrdinalIgnoreCase))
        {
            return isCreateLike ? "sample.user" : "tester.user";
        }

        if (propertyName.Contains("Code", StringComparison.OrdinalIgnoreCase))
        {
            return isCreateLike ? "NEWCODE" : UseCaseSeedDefaults.KnownCode;
        }

        if (propertyName.Contains("Status", StringComparison.OrdinalIgnoreCase))
        {
            return GetStatusValue(requestTypeName);
        }

        if (propertyName.Contains("Type", StringComparison.OrdinalIgnoreCase))
        {
            return requestTypeName.Contains("SupportTicket", StringComparison.OrdinalIgnoreCase)
                ? "Error"
                : "General";
        }

        if (propertyName.Contains("Language", StringComparison.OrdinalIgnoreCase))
        {
            return "En";
        }

        if (propertyName.Contains("Url", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownUrl;
        }

        if (propertyName.Contains("Avatar", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownAvatar;
        }

        if (propertyName.Contains("Headline", StringComparison.OrdinalIgnoreCase))
        {
            return "Experienced English Tutor";
        }

        if (propertyName.Contains("Description", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("Bio", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("Notes", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("Text", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("Address", StringComparison.OrdinalIgnoreCase))
        {
            return "Sample description";
        }

        if (propertyName.Contains("School", StringComparison.OrdinalIgnoreCase))
        {
            return "Sample High School";
        }

        if (propertyName.Contains("Grade", StringComparison.OrdinalIgnoreCase))
        {
            return "Grade 10";
        }

        if (propertyName.Contains("Class", StringComparison.OrdinalIgnoreCase))
        {
            return "Class A";
        }

        if (propertyName.Contains("Subject", StringComparison.OrdinalIgnoreCase))
        {
            return "English Basics";
        }

        if (propertyName.Contains("Name", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("Title", StringComparison.OrdinalIgnoreCase))
        {
            return propertyName.Contains("Last", StringComparison.OrdinalIgnoreCase)
                ? "Tester"
                : "Sample";
        }

        return "SampleValue";
    }

    private static string GetSeedStringValue(string propertyName)
    {
        if (propertyName.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownEmail;
        }

        if (propertyName.Contains("PasswordHash", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownPasswordHash;
        }

        if (propertyName.Contains("Password", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownPassword;
        }

        if (propertyName.Contains("Phone", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownPhone;
        }

        if (propertyName.Contains("Code", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownCode;
        }

        if (propertyName.Contains("Status", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownStatus;
        }

        if (propertyName.Contains("Url", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownUrl;
        }

        if (propertyName.Contains("Avatar", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownAvatar;
        }

        if (propertyName.Contains("FileName", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownFileName;
        }

        if (propertyName.Contains("ContentType", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownContentType;
        }

        return UseCaseSeedDefaults.KnownString;
    }

    private static string? GetDuplicateStringValue(string requestTypeName, string propertyName)
    {
        if (propertyName.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownEmail;
        }

        if (propertyName.Contains("UserName", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownString;
        }

        if (propertyName.Contains("Code", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownCode;
        }

        if (propertyName.Equals("Name", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("Title", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownString;
        }

        if (propertyName.Contains("Type", StringComparison.OrdinalIgnoreCase))
        {
            return GetSeedStringValue(propertyName);
        }

        if (propertyName.Contains("Phone", StringComparison.OrdinalIgnoreCase)
            && (requestTypeName.Contains("UpdateUser", StringComparison.OrdinalIgnoreCase)
                || requestTypeName.Contains("CreateUser", StringComparison.OrdinalIgnoreCase)))
        {
            return UseCaseSeedDefaults.KnownPhone;
        }

        return null;
    }

    private static DateTime GetDateTimeValue(string requestTypeName, string propertyName, bool valid)
    {
        if (propertyName.Contains("RecordingStarted", StringComparison.OrdinalIgnoreCase))
        {
            return valid ? DateTime.UtcNow.AddHours(-2) : DateTime.UtcNow.AddHours(2);
        }

        if (propertyName.Contains("RecordingEnded", StringComparison.OrdinalIgnoreCase))
        {
            return valid ? DateTime.UtcNow.AddHours(-1) : DateTime.UtcNow.AddHours(-3);
        }

        if (propertyName.Contains("StartTime", StringComparison.OrdinalIgnoreCase)
            && requestTypeName.Contains("Lesson", StringComparison.OrdinalIgnoreCase))
        {
            return valid ? DateTime.UtcNow.AddHours(1) : DateTime.UtcNow.AddHours(-1);
        }

        if (propertyName.Contains("EndTime", StringComparison.OrdinalIgnoreCase)
            && requestTypeName.Contains("Lesson", StringComparison.OrdinalIgnoreCase))
        {
            return valid ? DateTime.UtcNow.AddHours(2) : DateTime.UtcNow.AddHours(-2);
        }

        if (!valid)
        {
            if (propertyName.Contains("End", StringComparison.OrdinalIgnoreCase))
            {
                return UseCaseSeedDefaults.KnownUtcDateTime.AddHours(-1);
            }

            return UseCaseSeedDefaults.KnownUtcDateTime.AddHours(1);
        }

        if (propertyName.Contains("End", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownUtcDateTime.AddHours(1);
        }

        return UseCaseSeedDefaults.KnownUtcDateTime;
    }

    private static TimeOnly GetTimeOnlyValue(string propertyName, bool valid)
    {
        if (!valid)
        {
            return propertyName.Contains("End", StringComparison.OrdinalIgnoreCase)
                ? new TimeOnly(8, 0)
                : new TimeOnly(10, 0);
        }

        return propertyName.Contains("End", StringComparison.OrdinalIgnoreCase)
            ? new TimeOnly(10, 0)
            : new TimeOnly(8, 0);
    }

    private static int GetIntegerValue(string propertyName, bool valid)
    {
        if (!valid)
        {
            return propertyName.Contains("CoveragePercent", StringComparison.OrdinalIgnoreCase) ? 101 : 0;
        }

        if (propertyName.Contains("CoveragePercent", StringComparison.OrdinalIgnoreCase))
        {
            return 50;
        }

        if (propertyName.Contains("PageSize", StringComparison.OrdinalIgnoreCase))
        {
            return 10;
        }

        return 1;
    }

    private static string GetStatusValue(string requestTypeName)
    {
        if (requestTypeName.Contains("SupportTicket", StringComparison.OrdinalIgnoreCase))
        {
            return "Open";
        }

        if (requestTypeName.Contains("LessonRescheduleRequest", StringComparison.OrdinalIgnoreCase))
        {
            return "Pending";
        }

        if (requestTypeName.Contains("Lesson", StringComparison.OrdinalIgnoreCase))
        {
            return "Scheduled";
        }

        return "Active";
    }

    private static bool ShouldRecurseInto(Type propertyType)
    {
        return propertyType.IsClass
               && propertyType != typeof(string)
               && propertyType != typeof(Stream)
               && propertyType != typeof(byte[])
               && !typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyType);
    }

    private static object CreateFileUpload(string requestTypeName, bool valid)
    {
        var fileName = "test.txt";
        var contentType = "text/plain";

        if (requestTypeName.Contains("Cv", StringComparison.OrdinalIgnoreCase)
            || requestTypeName.Contains("Document", StringComparison.OrdinalIgnoreCase))
        {
            fileName = valid ? "resume.pdf" : "resume";
            contentType = "application/pdf";
        }
        else if (requestTypeName.Contains("Avatar", StringComparison.OrdinalIgnoreCase))
        {
            fileName = valid ? "avatar.png" : "avatar";
            contentType = "image/png";
        }
        else if (requestTypeName.Contains("Recording", StringComparison.OrdinalIgnoreCase))
        {
            fileName = valid ? "recording.webm" : "recording";
            contentType = "audio/webm";
        }
        else if (requestTypeName.Contains("IntroVideo", StringComparison.OrdinalIgnoreCase))
        {
            fileName = valid ? "intro.mp4" : "intro";
            contentType = "video/mp4";
        }

        var bytes = valid ? System.Text.Encoding.UTF8.GetBytes("hello world") : [];

        return new FileUpload
        {
            FileName = fileName,
            ContentType = contentType,
            Length = bytes.Length,
            Content = new MemoryStream(bytes)
        };
    }
}
