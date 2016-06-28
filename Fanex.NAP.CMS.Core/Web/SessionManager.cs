// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Core.Web
{
    using System;
    using System.Web;

    /// <summary>
    /// Gets and sets data to session.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Sets a value to session.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="sessionKey">Session key.</param>
        /// <param name="value">Value needs to be cached.</param>
        public static void Set<T>(string sessionKey, T value)
        {
            try
            {
                HttpContext.Current.Session[sessionKey] = value;
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        /// <summary>
        /// Gets a value from Session, then convert it to a specific type.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Value associated with passed key.</returns>
        public static T Get<T>(string sessionKey)
        {
            try
            {
                T value = (T)HttpContext.Current.Session[sessionKey];
                return value;
            }
            catch (IndexOutOfRangeException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Sets an object to the Session.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <param name="value">Value object.</param>
        public static void Set(string sessionKey, object value)
        {
            SessionManager.Set<object>(sessionKey, value);
        }

        /// <summary>
        /// Gets a value object from  the Session.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Value object associated with passed key.</returns>
        public static object Get(string sessionKey)
        {
            return SessionManager.Get<object>(sessionKey);
        }
    }
}