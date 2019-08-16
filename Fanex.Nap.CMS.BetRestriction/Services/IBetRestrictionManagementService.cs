namespace Fanex.Nap.CMS.BetRestriction.Services
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.BetRestriction.Criterias.Management;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels.Management;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IBetRestrictionManagementService
    {
        BetRestrictionSettingViewModel GetAllSettings(GetAllSettingCriteria criteria);

        string ParseEditSettingsXML(EditSettingsRequest request);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
