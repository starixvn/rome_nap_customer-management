// --------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.7.7">Created.</change>
// </history>

using System;

namespace Fanex.NAP.CMS.Core.Helpers
{
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
            return value.HasValue ? string.Format(ShortDateFormat, value) : string.Empty;
        }

        /// <summary>
        /// Converts a DateTime value to the full date time format.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>Standard full date string, such as: 10/10/2014 10:10:00 AM.</returns>
        public static string ToFullFormatedDate(this DateTime value)
        {
#pragma warning disable S2275 // Composite format strings should not lead to unexpected behavior at runtime
            return string.Format(FullDateTimeFormat, value);
#pragma warning restore S2275 // Composite format strings should not lead to unexpected behavior at runtime
        }

        /// <summary>
        /// Converts a DateTime value to the full date format.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>Standard full date string, such as: 10/10/2014 10:10:00 AM.</returns>
        public static string ToFullFormatedDate(this DateTime? value)
        {
#pragma warning disable S2275 // Composite format strings should not lead to unexpected behavior at runtime
            return value.HasValue ? string.Format(FullDateTimeFormat, value) : string.Empty;
#pragma warning restore S2275 // Composite format strings should not lead to unexpected behavior at runtime
        }
    }
}