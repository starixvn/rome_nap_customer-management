namespace Fanex.Nap.CMS.VirtualSettingControl.Services.ManualUpdateVRAccount
{
    using System.Collections.Generic;
    using Criteria.ManualUpdateVRAccount;
    using Infrastructure.Models;
    using Infrastructure.Services.Implementations;
    using Models.ManualUpdateVRAccount;
    using Resources;

    public class ManualUpdateVRAccountService : ServiceBase, IManualUpdateVRAccountService
    {
        public IEnumerable<ManualUpdateAccountDTO> GetManualUpdateAccounts(int sportType, string account)
            => GetRepositoryData<ManualUpdateAccountDTO>(
                new GetManualUpdateAccountListCriteria(sportType, account == null ? string.Empty : account.Trim()));

        public Result UnmarkManualUpdateAccount(int sportType, string custIds, int userId, string ip)
        {
            var result = ExecuteNonQuery(new UnmarkManualUpdateAccountsCriteria(sportType, custIds, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ESportsResource.UnmarkManualUpdateAccountSuccessfully;
            }

            return result;
        }
    }
}
