namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class LeagueGroup : IdNameModel
    {
        public LeagueGroup(short sportType, int id, string name)
        {
            SportType = sportType;
            Id = id;
            Name = name;
        }

        public short SportType { get; }

        public override bool Equals(object obj)
        {
            return ((LeagueGroup)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
