// --------------------------------------------------------------------------
// <copyright file="SportInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.Common
{
    using System;

    /// <summary>
    /// Sport information.
    /// </summary>
    [Serializable]
    public class SportInfo
    {
        public byte SportType { get; set; }

        public string SportName { get; set; }
    }
}