using Fanex.NAP.CMS.Services.Interfaces;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Core.Service.Interface;

namespace Fanex.NAP.CMS.Services.Implements
{
    public class PermissionSerivce : IPermissionSerivce
    {
        private readonly IUserService _userService;

        public PermissionSerivce(IUserService userService)
        {
            _userService = userService;
        }

        public bool HasPermission(int userId, string functionKey, Constant.Policy policy)
        {
            int functionPolicy = _userService.GetUserSiteFunctionKeyPolicy(userId, NapSettings.Settings.SiteId, functionKey);

            bool hasPermission = functionPolicy >= (int)policy;

            return hasPermission;
        }
    }
}