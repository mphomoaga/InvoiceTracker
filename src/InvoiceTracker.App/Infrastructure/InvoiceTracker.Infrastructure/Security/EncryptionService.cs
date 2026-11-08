using System.Security.Cryptography;
using System.Text;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace InvoiceTracker.Infrastructure.Security;

public class EncryptionService : IEncryptionService
{
   
    private readonly ISettingRepository _settingRepository;

    public EncryptionService(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public async Task<string> Encrypt(string value)
    {
        var encryptionKey = await _settingRepository.GetStringAsync("EcryptionKey");
        var key = SHA256.HashData(Encoding.UTF8.GetBytes(encryptionKey));
        if (string.IsNullOrEmpty(value))
            return value;

        byte[] nonce = RandomNumberGenerator.GetBytes(12);
        byte[] plainText = Encoding.UTF8.GetBytes(value);
        byte[] cipherText = new byte[plainText.Length];
        byte[] tag = new byte[16];

        using var aes = new AesGcm(key, tag.Length);

        aes.Encrypt(
            nonce,
            plainText,
            cipherText,
            tag);

        // Combine nonce, tag and encrypted value
        byte[] result = new byte[
            nonce.Length +
            tag.Length +
            cipherText.Length];

        Buffer.BlockCopy(
            nonce, 0,
            result, 0,
            nonce.Length);

        Buffer.BlockCopy(
            tag, 0,
            result, nonce.Length,
            tag.Length);

        Buffer.BlockCopy(
            cipherText, 0,
            result, nonce.Length + tag.Length,
            cipherText.Length);

        return Convert.ToBase64String(result);
    }

    public async Task<string> Decrypt(string encryptedValue)
    {
        if (string.IsNullOrEmpty(encryptedValue))
            return encryptedValue;

        byte[] encryptedData =
            Convert.FromBase64String(encryptedValue);

        if (encryptedData.Length < 28)
        {
            throw new CryptographicException(
                "The encrypted value is invalid.");
        }

        var key = SHA256.HashData(Encoding.UTF8.GetBytes(await _settingRepository.GetStringAsync("EcryptionKey")));
        byte[] nonce = encryptedData[..12];
        byte[] tag = encryptedData[12..28];
        byte[] cipherText = encryptedData[28..];
        byte[] plainText = new byte[cipherText.Length];

        using var aes = new AesGcm(key, tag.Length);

        aes.Decrypt(
            nonce,
            cipherText,
            tag,
            plainText);

        return Encoding.UTF8.GetString(plainText);
    }
}