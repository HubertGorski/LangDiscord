using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangDiscord.Services.Cache.Common
{
    public class TimedCache<TKey, TValue> where TKey : notnull
    {
        private class CacheEntry
        {
            public required TValue Value { get; init; }
            public DateTime Expiry { get; init; }
        }

        private readonly ConcurrentDictionary<TKey, CacheEntry> _storage = new();
        private readonly TimeSpan _defaultExpiry;
        private readonly Timer _cleanupTimer;
        private bool _disposed;


        public TimedCache(TimeSpan defaultExpiry, TimeSpan cleanupInterval)
        {
            _defaultExpiry = defaultExpiry;
            _cleanupTimer = new Timer(_ => ClearExpired(), null, cleanupInterval, cleanupInterval);
        }


        public void Set(TKey key, TValue value, TimeSpan? customExpiry = null)
        {
            var entry = new CacheEntry
            {
                Value = value,
                Expiry = DateTime.UtcNow.Add(customExpiry ?? _defaultExpiry)
            };

            _storage[key] = entry;
        }

        public Dictionary<TKey, (TValue Value, DateTime Expiry)> GetAllEntries()
        {
            return _storage.ToDictionary(
                kv => kv.Key,
                kv => (kv.Value.Value, kv.Value.Expiry)
            );
        }

        public bool TryGet(TKey key, out TValue? value)
        {
            if (_storage.TryGetValue(key, out var entry) && entry.Expiry > DateTime.UtcNow)
            {
                value = entry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryRemove(TKey key, out TValue? value)
        {
            if (_storage.TryRemove(key, out var entry))
            {
                value = entry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public int ClearExpired()
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _storage
                .Where(kv => kv.Value.Expiry <= now)
                .Select(kv => kv.Key)
                .ToList();

            int removed = 0;
            foreach (var key in expiredKeys)
            {
                if (_storage.TryRemove(key, out _))
                {
                    removed++;
                }
            }

            return removed;
        }
        public void Dispose()
        {
            if (_disposed) return;
            _cleanupTimer.Dispose();
            _storage.Clear();
            _disposed = true;
        }

        public int Count => _storage.Count;
        public void Clear() => _storage.Clear();

        //TODO: Zoptymalizować, zrobić bezpieczniej
        public bool TryGetRandomByPartialKey<TPartial>(TPartial partialKey, Func<TKey, TPartial, bool> keyMatcher, out TValue? value, out TKey? fullKey)
        {
            var now = DateTime.UtcNow;
            var matchingEntries = _storage
                .Where(kv => keyMatcher(kv.Key, partialKey) && kv.Value.Expiry > now)
                .ToList();

            if (matchingEntries.Count == 0)
            {
                value = default;
                fullKey = default;

                return false;
            }

            var random = new Random();
            var randomEntry = matchingEntries[random.Next(matchingEntries.Count)];

            value = randomEntry.Value.Value;
            fullKey = randomEntry.Key;

            return true;
        }

    }
}
