// --------------------------------------------------------------------------
// <copyright file="UpdatedSMAOffsetResult.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View modal contains the result of SMA offset after updating or removing.
    /// </summary>
    [Serializable]
    public class UpdatedSMAOffsetResult
    {
        public IEnumerable<SMAOffset> OffsetItems { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}