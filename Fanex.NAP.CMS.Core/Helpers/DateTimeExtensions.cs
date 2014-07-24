// --------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.7.7">Created.</change>
// </history>

namespace Fanex.NAP.CMS.Core.Helpers
{
    using System;

    /// <summary>
    /// Contains extensions of DateTime type.
    /// </summary>
    public static class DateTimeExtensions
    {
        private const string ShortDateFormat = "{0:MM/dd/yyyy}";
        private const string FullDateTimeFormat = "{0:MM/dd/yyyy h:mm:ss tt}";

        /// <summary>
        /// Converts a DateTime value to the short date format.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>Standard short date string, such as: 10/10/2014.</returns>
        public static string ToShortFormatedDate(this DateTime value)
        {
            return string.Format(ShortDateFormat, value);
        }

        /// <summary>
        /// Converts a DateTime value to the short date format.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>Standard short date string, such as: 10/10/2014.</returns>
        public static string ToShortFormatedDate(this DateTime? value)
        {
            if (value.HasValue)
            {
                return string.Format(ShortDateFormat, value);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a DateTime value to the full date time format.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>Standard full date string, such as: 10/10/2014 10:10:00 AM.</returns>
        public static string ToFullFormatedDate(this DateTime value)
        {
            return string.Format(FullDateTimeFormat, value);
        }

        /// <summary>
        /// Converts a DateTime value to the full date format.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>Standard full date string, such as: 10/10/2014 10:10:00 AM.</returns>
        public static string ToFullFormatedDate(this DateTime? value)
        {
            if (value.HasValue)
            {
                return string.Format(FullDateTimeFormat, value);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}