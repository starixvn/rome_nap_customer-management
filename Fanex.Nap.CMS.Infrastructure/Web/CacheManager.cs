// --------------------------------------------------------------------------
// <copyright file="CacheManager.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using System;
    using Fanex.Caching;

    /// <summary>
    /// Cache manager.
    /// </summary>
    public static class CacheManager
    {
        private static readonly ICache CacheManagerInstance = CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        /// Gets cached value associated with the passed key.
        /// </summary>
        /// <param name="key">Key of cached value.</param>
        /// <returns>Cached value.</returns>
        public static object Get(string key)
        {
            return CacheManagerInstance.Get<object>(key);
        }

        /// <summary>
        /// Gets cached value which is associated with the passed key and converts it to the expected type.
        /// </summary>
        /// <typeparam name="T">Expected type.</typeparam>
        /// <param name="key">Key of cached value.</param>
        /// <returns>Cached value.</returns>
        public static T Get<T>(string key)
        {
            return CacheManagerInstance.Get<T>(key);
        }

        /// <summary>
        /// Sets a value to specific key.
        /// </summary>
        /// <param name="key">Key that indicates for the value when caching.</param>
        /// <param name="value">Value that needs to be cached.</param>
        public static void Set(string key, object value)
        {
            CacheManagerInstance.Set(key, value);
        }

        /// <summary>
        /// Caches a value.
        /// </summary>
        /// <typeparam name="T">Type of cached value.</typeparam>
        /// <param name="key">The key which indicates the cached value.</param>
        /// <param name="value">Value needs to be cached.</param>
        public static void Set<T>(string key, T value)
        {
            CacheManagerInstance.Set<T>(key, value);
        }

        /// <summary>
        /// Caches a value.
        /// </summary>
        /// <param name="key">The key which indicates the cached value.</param>
        /// <param name="value">Value needs to be cached.</param>
        /// <param name="timeSpanExpiration">The time span which the cached value will be expired.</param>
        public static void Set(string key, object value, TimeSpan timeSpanExpiration)
        {
            CacheManagerInstance.Set(key, value, timeSpanExpiration);
        }

        /// <summary>
        /// Caches a value.
        /// </summary>
        /// <typeparam name="T">Type of cached value.</typeparam>
        /// <param name="key">The key which indicates the cached value.</param>
        /// <param name="value">Value needs to be cached.</param>
        /// <param name="timeSpanExpiration">The time span which the cached value will be expired.</param>
        public static void Set<T>(string key, T value, TimeSpan timeSpanExpiration)
        {
            CacheManagerInstance.Set<T>(key, value, timeSpanExpiration);
        }

        /// <summary>
        /// Caches a value.
        /// </summary>
        /// <param name="key">The key which indicates the cached value.</param>
        /// <param name="value">Value needs to be cached.</param>
        /// <param name="absoluteExpiration">The time when the cached value will be expired.</param>
        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            CacheManagerInstance.Set(key, value, absoluteExpiration);
        }

        /// <summary>
        /// Caches a value.
        /// </summary>
        /// <typeparam name="T">Type of cached value.</typeparam>
        /// <param name="key">The key which indicates the cached value.</param>
        /// <param name="value">Value needs to be cached.</param>
        /// <param name="absoluteExpiration">The time when the cached value will be expired.</param>
        public static void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            CacheManagerInstance.Set<T>(key, value, absoluteExpiration);
        }

        /// <summary>
        /// Removes a cached value of a key.
        /// </summary>
        /// <param name="key">Key of value that needs to be removed.</param>
        public static void Remove(string key)
        {
            CacheManagerInstance.Remove(key);
        }
    }
}