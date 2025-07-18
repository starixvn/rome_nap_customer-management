namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;

    public class ValidateBetLimitSettingResponse
    {
        public List<CompanyBetLimitSettingExcel> BetLimits { get; set; }

        public Result Error { get; set; }
    }
}
