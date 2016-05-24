using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Server.Api.Filters.Data.Cache
{
    /// <summary>
    /// Inspired by: 
    /// https://msdn.microsoft.com/library/ff919782(v=vs.100).aspx
    /// </summary>
    public class MemoryCacheImplementation : MemoryCache, ICacheImplementation
    {
        private const string DefaultCustomCache = nameof(DefaultCustomCache);

        public MemoryCacheImplementation()
            : base(DefaultCustomCache)
        {
        }

        public int TotalEntries
        {
            get { return (int)GetCount(); }
        }

        public void Add<TResponseOutput>(string key, TResponseOutput data, TimeSpan duration)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(duration.TotalMilliseconds);

            Set(key, data, policy);
        }

        public TResponseOutput Get<TResponseOutput>(string key)
        {
            var data = Get(key);

            if (data != null)
            {
                return (TResponseOutput)data;
            }

            return default(TResponseOutput);
        }

        public bool DoesKeyExist(string key)
        {
            return Contains(key);
        }

        public void DisposeCache()
        {
            Dispose();
        }
    }
}
