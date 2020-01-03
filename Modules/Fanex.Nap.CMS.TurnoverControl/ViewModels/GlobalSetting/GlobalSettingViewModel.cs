using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting
{
    public class GlobalSettingViewModel
    {
        public GlobalSettingViewModel(
            short sportType,
            bool hostPermission,
            IEnumerable<IdNameModel> currencies = null,
            IEnumerable<IdNameModel> profiles = null)
        {
            SportType = sportType;
            HostPermission = hostPermission;
            Currencies = currencies;
            Profiles = profiles;
        }

        public short SportType { get; }

        public bool HostPermission { get; }

        public IEnumerable<IdNameModel> Currencies { get; }

        public IEnumerable<IdNameModel> Profiles { get; }
    }
}
