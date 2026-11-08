namespace InvoiceTracker.Application.Interfaces.Services
{
    public interface IEncryptionService
    {
        public Task<string> Encrypt(string value);
        Task<string> Decrypt(string encryptedValue);
    }
}
