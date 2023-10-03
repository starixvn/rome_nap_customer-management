namespace Fanex.Nap.CMS.ManageProductSetting.Repositories.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
    public interface ICompanyBetLimitRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);
        IEnumerable<IdNameModel> GetProducts(ICriteria criteria);
        IEnumerable<CompanyBetLimitSetting> GetBetLimitSettings(ICriteria criteria);
    }
}