// --------------------------------------------------------------------------
// <copyright file="TurnoverInfoByGroup.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    /// <summary>
    /// Turnover Information By Group.
    /// </summary>
    [Serializable]
    public class TurnoverInfoByGroup : IdNameModel
    {
        public CustomerTurnoverInfo TurnoverInfo { get; set; }

        public List<TurnoverInfoByGroup> ChildTurnoverInfo { get; set; }
    }
}
