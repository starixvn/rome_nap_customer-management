using System;
using System.Collections;
using System.Collections.Generic;
namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

    public class AddMultipleGlobalSettingCriteria : CommandBaseCriteria
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<GlobalSettingCustomer> GlobalSettings { get; }
        public string GlobalSetting {  get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingInsertMultiple";

        
    }
}
