namespace Fanex.NAP.CMS.BetLimitTypeControl.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    
    public class LeagueGroup : IdNameModel
    {
        public LeagueGroup() { }

        public LeagueGroup(short groupId, string groupName, short sportType)
        {
            SportType = sportType;
            Id = groupId;
            Name = groupName;
        }

        public short SportType { get; set; }
    }
}
