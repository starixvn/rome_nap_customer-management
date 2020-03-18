using System;

namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    [Serializable]
    public class MaxTurnoverItem
    {
        public MaxTurnoverItem()
        {
        }

        public MaxTurnoverItem(int profileId, decimal maxTurnover)
        {
            ProfileId = profileId;
            MaxTurnover = maxTurnover;
        }

        public int ProfileId { get; set; }

        public decimal MaxTurnover { get; set; }
    }
}
