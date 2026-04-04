using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Infrastructure.Security.Encryption;
using Microsoft.Extensions.Options;
using Xunit;

namespace EngConnect.Tests.EngConnect.BuildingBlocks.Infrastructure.Security.Encryption;

public class AesEncryptionServiceTests
{
    [Fact]
    public void Encrypt_and_decrypt_round_trip_plain_text()
    {
        var settings = CreateSettings();
        var sut = new AesEncryptionService(Options.Create(settings));

        var cipherText = sut.Encrypt("engconnect");
        var plainText = sut.Decrypt(cipherText);

        Assert.NotEqual("engconnect", cipherText);
        Assert.Equal("engconnect", plainText);
    }

    [Fact]
    public void Constructor_throws_when_key_or_iv_is_not_valid_base64()
    {
        var settings = new EncryptionSettings
        {
            Key = "not-base64",
            IV = "not-base64"
        };

        var exception = Assert.Throws<ArgumentException>(() => new AesEncryptionService(Options.Create(settings)));

        Assert.Contains("Base64", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    private static EncryptionSettings CreateSettings()
    {
        return new EncryptionSettings
        {
            Key = Convert.ToBase64String(Enumerable.Range(1, 32).Select(index => (byte)index).ToArray()),
            IV = Convert.ToBase64String(Enumerable.Range(1, 16).Select(index => (byte)(index + 32)).ToArray())
        };
    }
}
