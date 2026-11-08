namespace InvoiceTracker.Application.Interfaces.Repositories
{
    public interface ISettingRepository
    {
        public Task<string?> GetStringAsync(string key);
    }
}
