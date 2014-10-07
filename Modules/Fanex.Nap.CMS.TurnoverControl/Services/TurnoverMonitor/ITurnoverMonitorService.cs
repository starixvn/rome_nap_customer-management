// --------------------------------------------------------------------------
// <copyright file="ITurnoverMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;

    public interface ITurnoverMonitorService
    {
        List<IdNameModel> Sports(GetSportsCriteria criteria);

        List<IdNameModel> Leagues(GetLeaguesCriteria criteria);

        TurnoverInfoByGroup SMATurnoverList(GetSMATurnoverListCriteria criteria);

        List<TurnoverBalance> CompanyTurnover(GetCompanyTurnoverCriteria criteria);

        Result UpdateCustomerMaxTurnoverByMatch(UpdateMaxTurnoverByMatchCriteria criteria);
    }
}
