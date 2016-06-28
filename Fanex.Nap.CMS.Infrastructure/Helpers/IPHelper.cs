// --------------------------------------------------------------------------
// <copyright file="IPHelper.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Helpers
{
    using System;
    using System.Web;
    using Fanex.Common;

    /// <summary>
    /// IP Helper.
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// Gets the client IP address.
        /// </summary>
        /// <returns>The client IP address.</returns>
        public static string ClientIP()
        {
            try
            {
                if (HttpContext.Current.Request.IsLocal)
                {
                    return "127.0.0.1";
                }

                return Converter.ToString2(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
