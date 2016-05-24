using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Server.Api.Filters.Data.Cache
{
    public interface ICacheImplementation
    {
        int TotalEntries { get; }

        /// <summary>
        /// Adds data to cache
        /// </summary>
        void Add<TResponseOutput>(string key, TResponseOutput data, TimeSpan duration);

        /// <summary>
        /// Retrives data from cache
        /// </summary>
        TResponseOutput Get<TResponseOutput>(string key);

        /// <summary>
        /// Validates whether or not the key exists.
        /// </summary>
        bool DoesKeyExist(string key);

        /// <summary>
        /// Disposes the cache object
        /// </summary>
        void DisposeCache();
    }
}
