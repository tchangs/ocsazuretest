using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTest
{
    public static class MemLoader
    {
        public const string CACHE_VALUE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-=~!@#$%^&*()_+";
        public static List<LoadTest> GetMemoryLoadTest(int iternations, int slidingExpiration, string cacheValue= MemLoader.CACHE_VALUE)
        {
            List<LoadTest> loadTests = new List<LoadTest>();
            var rng = new Random();
            for (int i = 0; i <= iternations; i++)
            {
                LoadTest loadTest = new LoadTest();
                loadTest.LoadTaskAction = "MemUsages";
                var cache = new MemoryCache(new MemoryCacheOptions());
                var cacheEntry = cache.CreateEntry("memoryloadtest-key");
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(slidingExpiration);
                cacheEntry.SetValue(loadTests);
                int size = rng.Next(5000, 10000);
                cacheEntry.SetSize(size);
                cacheEntry.SetValue(cacheValue);
                loadTest.MemoryCached = size;
                loadTest.Message = "MemoryCachedValue=" + cacheValue;
                loadTests.Add(loadTest);
            }

            return loadTests;
        }

    }
}
