using Fanex.NAP.Client.Helpers.Business;

namespace Fanex.NAP.CMS.Services.Interfaces
{
    public interface IPermissionSerivce
    {
        bool HasPermission(int userId, string functionKey, Constant.Policy policy);
    }
}
