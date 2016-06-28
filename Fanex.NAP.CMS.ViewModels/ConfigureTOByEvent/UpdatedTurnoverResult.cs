// --------------------------------------------------------------------------
// <copyright file="UpdatedTurnoverResult.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    using System.Collections.Generic;

    /// <summary>
    /// View model which is used to save the turnover list after a updating call.
    /// </summary>
    public class UpdatedTurnoverResult
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public object Turnovers { get; set; }
    }
}