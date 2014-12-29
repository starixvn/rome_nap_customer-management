// --------------------------------------------------------------------------
// <copyright file="IGlobalSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;

    public interface IGlobalSettingRepository
    {
        List<CustomerInfo> GlobalSettingList(GlobalSettingListCriteria criteria);

        Result EditGlobalSetting(EditGlobalSettingCriteria criteria);

        Result AddGlobalSetting(AddGlobalSettingCriteria criteria);

        Result DeleteGlobalSetting(DeleteGlobalSettingCriteria criteria);
    }
}
