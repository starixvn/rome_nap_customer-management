namespace Fanex.Nap.CMS.VirtualSettingControl.Services.ManualUpdateVRAccount
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.ManualUpdateVRAccount;

    public interface IManualUpdateVRAccountService
    {
        IEnumerable<ManualUpdateAccountDTO> GetManualUpdateAccounts(int sportType, string account);

        Result UnmarkManualUpdateAccount(int sportType, string custIds, int userId, string ip);
    }
}
