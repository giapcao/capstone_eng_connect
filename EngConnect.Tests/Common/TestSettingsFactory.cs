using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;

namespace EngConnect.Tests.Common;

internal static class TestSettingsFactory
{
    private static readonly Lazy<(string PrivateKey, string PublicKey)> JwtKeys = new(() =>
    {
        var (privatePem, publicPem) = RsaKeyGenerator.GenerateKeyPair();
        return
        (
            Convert.ToBase64String(Encoding.UTF8.GetBytes(privatePem)),
            Convert.ToBase64String(Encoding.UTF8.GetBytes(publicPem))
        );
    });

    public static T Create<T>() where T : class, new()
    {
        return (T)Create(typeof(T));
    }

    public static object Create(Type settingsType)
    {
        if (settingsType == typeof(JwtSettings))
        {
            return new JwtSettings
            {
                PrivateKeyBytes = JwtKeys.Value.PrivateKey,
                PublicKeyBytes = JwtKeys.Value.PublicKey,
                AccessTokenExpirationMinutes = 30,
                RefreshTokenExpirationMinutes = 60,
                Issuer = "engconnect.tests",
                Audience = "engconnect.clients"
            };
        }

        if (settingsType == typeof(EncryptionSettings))
        {
            return new EncryptionSettings
            {
                Key = Convert.ToBase64String(Enumerable.Range(1, 32).Select(index => (byte)index).ToArray()),
                IV = Convert.ToBase64String(Enumerable.Range(1, 16).Select(index => (byte)(index + 32)).ToArray())
            };
        }

        if (settingsType == typeof(RedirectUrlSettings))
        {
            return new RedirectUrlSettings
            {
                GoogleLoginFailedUrl = "https://app.example/failure",
                GoogleLoginSuccessUrl = "https://app.example/success"
            };
        }

        if (settingsType == typeof(GitHubModelsSettings))
        {
            return new GitHubModelsSettings
            {
                Endpoint = "https://models.example/",
                Token = "test-token",
                ModelName = "test-model"
            };
        }

        if (settingsType == typeof(WhisperApiSettings))
        {
            return new WhisperApiSettings
            {
                Endpoint = "https://speech.example/"
            };
        }

        if (settingsType == typeof(AwsStorageSettings))
        {
            return new AwsStorageSettings
            {
                AccessKey = "access-key",
                SecretKey = "secret-key",
                Region = "ap-southeast-1",
                BucketName = "engconnect-tests",
                CloudFront = "https://cdn.example/"
            };
        }

        if (settingsType == typeof(GoogleDriveSettings))
        {
            return new GoogleDriveSettings
            {
                ApplicationName = "EngConnect.Tests",
                ClientId = "client-id",
                ClientSecret = "client-secret",
                RefreshToken = "refresh-token",
                MainFolderId = "main-folder-id"
            };
        }

        if (settingsType == typeof(GmailApiSettings))
        {
            return new GmailApiSettings
            {
                ApplicationName = "EngConnect.Tests",
                ClientId = "client-id",
                ClientSecret = "client-secret",
                RefreshToken = "refresh-token",
                SenderEmail = "no-reply@example.com",
                SenderName = "EngConnect Tests"
            };
        }

        if (settingsType == typeof(FileStorageSettings))
        {
            return new FileStorageSettings
            {
                BaseUrl = "https://files.example",
                MaxFileSize = 5 * 1024 * 1024,
                UseUniqueFilenames = true,
                PublicFileBasePath = Path.Combine(Path.GetTempPath(), "engconnect-tests", "public"),
                PrivateFileBasePath = Path.Combine(Path.GetTempPath(), "engconnect-tests", "private")
            };
        }

        var instance = Activator.CreateInstance(settingsType)
                       ?? throw new InvalidOperationException($"Cannot create settings type {settingsType.FullName}.");

        PopulateSimpleProperties(instance);
        return instance;
    }

    private static void PopulateSimpleProperties(object instance)
    {
        var properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var property in properties.Where(property => property.CanWrite))
        {
            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            var currentValue = property.GetValue(instance);

            if (propertyType == typeof(string) && string.IsNullOrWhiteSpace(currentValue as string))
            {
                property.SetValue(instance, "test-value");
            }
            else if (propertyType == typeof(int) && Equals(currentValue, 0))
            {
                property.SetValue(instance, 1);
            }
            else if (propertyType == typeof(long) && Equals(currentValue, 0L))
            {
                property.SetValue(instance, 1L);
            }
            else if (propertyType == typeof(double) && Equals(currentValue, 0d))
            {
                property.SetValue(instance, 1d);
            }
            else if (propertyType == typeof(decimal) && Equals(currentValue, 0m))
            {
                property.SetValue(instance, 1m);
            }
            else if (propertyType == typeof(bool))
            {
                property.SetValue(instance, true);
            }
        }
    }
}
