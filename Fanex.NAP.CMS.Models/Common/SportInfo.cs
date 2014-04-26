// --------------------------------------------------------------------------
// <copyright file="SportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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