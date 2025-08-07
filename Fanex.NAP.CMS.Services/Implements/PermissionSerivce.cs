using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Services;
using Fanex.NAP.CMS.Services.Interfaces;

namespace Fanex.NAP.CMS.Services.Implements
{
    public class PermissionSerivce : IPermissionSerivce
    {
        private readonly IUserService _userService;
        private readonly INapSettings _napSettings;

        public PermissionSerivce() :
            this(new UserService(), new NapSettings())
        {
        }

        public PermissionSerivce(IUserService userService, INapSettings napSettings)
        {
            _userService = userService;
            _napSettings = napSettings;
        }

        public bool HasPermission(int userId, string functionKey, Constant.Policy policy)
        {
            int functionPolicy = _userService.GetPermission(userId, _napSettings.SiteId, functionKey);

            bool hasPermission = functionPolicy >= (int)policy;

            return hasPermission;
        }
    }
}