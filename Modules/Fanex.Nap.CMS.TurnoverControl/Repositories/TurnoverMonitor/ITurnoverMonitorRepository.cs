// --------------------------------------------------------------------------
// <copyright file="ITurnoverMonitorRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;

    public interface ITurnoverMonitorRepository : IBaseRepository
    {
        List<IdNameModel> Sports(GetSportsCriteria criteria);

        List<IdNameModel> Leagues(GetLeaguesCriteria criteria);

        List<CustomerTurnoverInfo> SMATurnoverList(GetSMATurnoverListCriteria criteria);
    }
}
