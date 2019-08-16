using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.BetRestriction.Requests
{
    public class EditSettingsRequest
    {
        public List<EditSettingRequest> EditSettings { get; set; }

        public EditSettingsRequest()
        {
            EditSettings = new List<EditSettingRequest>();
        }
    }

    public class EditSettingRequest
    {
        public int SettingId { get; set; }

        public bool? LiveBetAcceptStatus { get; set; }

        public bool? SeizeBetTicketStatus { get; set; }

        public bool? BetLimitStatus { get; set; }
    }
}
