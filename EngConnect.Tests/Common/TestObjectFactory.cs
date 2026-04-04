using System.Collections;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EngConnect.Tests.Common;

internal static class TestObjectFactory
{
    public static object? CreateValue(Type type, int depth = 0)
    {
        if (depth > 4)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        var underlyingType = Nullable.GetUnderlyingType(type);
        if (underlyingType != null)
        {
            return CreateValue(underlyingType, depth + 1);
        }

        if (type == typeof(string))
        {
            return UseCaseSeedDefaults.KnownString;
        }

        if (type == typeof(Guid))
        {
            return UseCaseSeedDefaults.KnownGuid;
        }

        if (type == typeof(int))
        {
            return 1;
        }

        if (type == typeof(long))
        {
            return 1L;
        }

        if (type == typeof(double))
        {
            return 1d;
        }

        if (type == typeof(decimal))
        {
            return 1m;
        }

        if (type == typeof(float))
        {
            return 1f;
        }

        if (type == typeof(bool))
        {
            return true;
        }

        if (type == typeof(DateTime))
        {
            return UseCaseSeedDefaults.KnownUtcDateTime;
        }

        if (type == typeof(DateTimeOffset))
        {
            return UseCaseSeedDefaults.KnownUtcDateTimeOffset;
        }

        if (type == typeof(DateOnly))
        {
            return DateOnly.FromDateTime(UseCaseSeedDefaults.KnownUtcDateTime);
        }

        if (type == typeof(TimeOnly))
        {
            return TimeOnly.FromDateTime(UseCaseSeedDefaults.KnownUtcDateTime);
        }

        if (type == typeof(TimeSpan))
        {
            return TimeSpan.FromMinutes(5);
        }

        if (type == typeof(Uri))
        {
            return new Uri("https://example.com/test");
        }

        if (type == typeof(byte[]))
        {
            return Encoding.UTF8.GetBytes("test-data");
        }

        if (type == typeof(Stream))
        {
            return new MemoryStream(Encoding.UTF8.GetBytes("test-data"));
        }

        if (type == typeof(CancellationToken))
        {
            return CancellationToken.None;
        }

        if (type == typeof(IFormFile))
        {
            return CreateFormFile();
        }

        if (type == typeof(ClaimsPrincipal))
        {
            return CreatePrincipal();
        }

        if (type == typeof(HttpClient))
        {
            return CreateHttpClient();
        }

        if (type == typeof(FileReadResult))
        {
            return new FileReadResult
            {
                Stream = new MemoryStream(Encoding.UTF8.GetBytes("download")),
                FileName = "download.txt",
                ContentType = "text/plain",
                Size = 8,
                RelativePath = "downloads/download.txt",
                IsPrivate = true
            };
        }

        if (type == typeof(FileUploadResult))
        {
            return new FileUploadResult
            {
                OriginalFileName = "test.txt",
                StoredFileName = "stored-test.txt",
                Size = 8,
                ContentType = "text/plain",
                RelativePath = "uploads/test.txt",
                RelativePathSystem = "uploads/test.txt",
                Url = "https://files.example/uploads/test.txt"
            };
        }

        if (type == typeof(FileUpload))
        {
            return new FileUpload
            {
                FileName = UseCaseSeedDefaults.KnownFileName,
                ContentType = UseCaseSeedDefaults.KnownContentType,
                Length = 11,
                Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world"))
            };
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IOptions<>))
        {
            return CreateOptions(type.GenericTypeArguments[0]);
        }

        if (type.IsEnum)
        {
            return Enum.GetValues(type).GetValue(0);
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType()!;
            var array = Array.CreateInstance(elementType, 1);
            array.SetValue(CreateValue(elementType, depth + 1), 0);
            return array;
        }

        if (TryCreateCollection(type, depth, out var collection))
        {
            return collection;
        }

        if (type.IsInterface || type.IsAbstract)
        {
            return null;
        }

        var constructor = type
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .OrderBy(ctor => ctor.GetParameters().Length)
            .FirstOrDefault();

        object? instance;
        if (constructor != null)
        {
            var arguments = constructor
                .GetParameters()
                .Select(parameter => CreateValue(parameter.ParameterType, depth + 1))
                .ToArray();

            instance = constructor.Invoke(arguments);
        }
        else
        {
            instance = Activator.CreateInstance(type);
        }

        if (instance != null)
        {
            PopulateWritableProperties(instance, depth + 1);
        }

        return instance;
    }

    public static T CreateValue<T>() where T : class
    {
        return (T)(CreateValue(typeof(T)) ?? throw new InvalidOperationException($"Cannot create {typeof(T).FullName}."));
    }

    public static ClaimsPrincipal CreatePrincipal()
    {
        var userId = UseCaseSeedDefaults.KnownGuid;
        var tutorId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var studentId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var claims = new[]
        {
            new Claim("sub", userId.ToString()),
            new Claim("userId", userId.ToString()),
            new Claim("studentId", studentId.ToString()),
            new Claim("tutorId", tutorId.ToString()),
            new Claim("email", "tester@example.com"),
            new Claim(ClaimTypes.Role, "Tutor"),
            new Claim("role", "Tutor")
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }

    public static IFormFile CreateFormFile(
        string fileName = "test.txt",
        string contentType = "text/plain",
        string content = "hello world")
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    public static HttpClient CreateHttpClient(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? handler = null)
    {
        return new HttpClient(new StubHttpMessageHandler(handler))
        {
            BaseAddress = new Uri("https://example.com/")
        };
    }

    public static object CreateOptions(Type settingsType)
    {
        var settings = TestSettingsFactory.Create(settingsType);
        var createMethod = typeof(Options)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == nameof(Options.Create) && method.IsGenericMethodDefinition);

        return createMethod.MakeGenericMethod(settingsType).Invoke(null, [settings])!;
    }

    private static bool TryCreateCollection(Type type, int depth, out object? collection)
    {
        collection = null;

        if (!type.IsGenericType)
        {
            return false;
        }

        var definition = type.GetGenericTypeDefinition();
        var genericArguments = type.GetGenericArguments();

        if (definition == typeof(List<>)
            || definition == typeof(IList<>)
            || definition == typeof(IEnumerable<>)
            || definition == typeof(ICollection<>)
            || definition == typeof(IReadOnlyList<>))
        {
            var listType = typeof(List<>).MakeGenericType(genericArguments[0]);
            var list = (IList)Activator.CreateInstance(listType)!;
            list.Add(CreateValue(genericArguments[0], depth + 1));
            collection = list;
            return true;
        }

        if (definition == typeof(Dictionary<,>)
            || definition == typeof(IDictionary<,>)
            || definition == typeof(IReadOnlyDictionary<,>))
        {
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);
            var dictionary = Activator.CreateInstance(dictionaryType)!;
            var addMethod = dictionaryType.GetMethod("Add", genericArguments)!;
            addMethod.Invoke(dictionary,
            [
                CreateValue(genericArguments[0], depth + 1)!,
                CreateValue(genericArguments[1], depth + 1)
            ]);
            collection = dictionary;
            return true;
        }

        return false;
    }

    private static void PopulateWritableProperties(object instance, int depth)
    {
        if (depth > 3)
        {
            return;
        }

        foreach (var property in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanWrite || property.GetIndexParameters().Length != 0)
            {
                continue;
            }

            object? currentValue;
            try
            {
                currentValue = property.GetValue(instance);
            }
            catch
            {
                continue;
            }

            if (!ShouldPopulate(property.PropertyType, currentValue, instance.GetType()))
            {
                continue;
            }

            var value = TryCreateNamedValue(property);
            if (value == null)
            {
                value = CreateValue(property.PropertyType, depth + 1);
            }
            if (value == null)
            {
                continue;
            }

            try
            {
                property.SetValue(instance, value);
            }
            catch
            {
                // Ignore init-only and non-settable runtime cases.
            }
        }
    }

    private static bool ShouldPopulate(Type propertyType, object? currentValue, Type declaringType)
    {
        if (propertyType == declaringType)
        {
            return false;
        }

        if (currentValue == null)
        {
            return true;
        }

        if (propertyType == typeof(string))
        {
            return string.IsNullOrWhiteSpace((string)currentValue);
        }

        if (propertyType == typeof(Guid))
        {
            return (Guid)currentValue == Guid.Empty;
        }

        return false;
    }

    private static object? TryCreateNamedValue(PropertyInfo property)
    {
        var propertyName = property.Name;
        var propertyType = property.PropertyType;

        if (propertyType == typeof(string))
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

        if (propertyType == typeof(Guid))
        {
            return UseCaseSeedDefaults.KnownGuid;
        }

        if (propertyType == typeof(Guid?))
        {
            return UseCaseSeedDefaults.KnownGuid;
        }

        if (propertyType == typeof(DateTime))
        {
            return UseCaseSeedDefaults.KnownUtcDateTime;
        }

        if (propertyType == typeof(DateTimeOffset))
        {
            return UseCaseSeedDefaults.KnownUtcDateTimeOffset;
        }

        return null;
    }
}
