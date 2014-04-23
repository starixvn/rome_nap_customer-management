// --------------------------------------------------------------------------
// <copyright file="UpdateSportTurnoverFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    /// <summary>
    /// The filter contains values that are used to update turnover of sport and get the sport list after updating.
    /// </summary>
    public class UpdateSportTurnoverFilter
    {
        public int SportID { get; set; }

        public int Value { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }
    }
}