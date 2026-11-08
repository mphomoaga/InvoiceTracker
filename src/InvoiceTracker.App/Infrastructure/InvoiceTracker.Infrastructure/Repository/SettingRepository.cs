using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InvoiceTracker.Infrastructure.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private readonly InvoiceTrackerDbContext _context;
        private readonly IMemoryCache _memoryCache;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private const string CacheKeyPrefix = "setting:";

        public SettingRepository(
            InvoiceTrackerDbContext context,
            IMemoryCache memoryCache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public Task<string?> GetStringAsync(string key)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            return _memoryCache.GetOrCreateAsync(
                $"{CacheKeyPrefix}{key}",
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;

                    return await _context.Settings
                        .AsNoTracking()
                        .Where(setting => setting.IsActive && setting.Key == key)
                        .Select(setting => setting.Value)
                        .FirstOrDefaultAsync();
                })!;
        }
    }
}
