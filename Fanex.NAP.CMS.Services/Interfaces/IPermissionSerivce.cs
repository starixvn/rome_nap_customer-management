using Fanex.NAP.Core.Business;

namespace Fanex.NAP.CMS.Services.Interfaces
{
    public interface IPermissionSerivce
    {
        bool HasPermission(int userId, string functionKey, Constant.Policy policy);
    }
}
