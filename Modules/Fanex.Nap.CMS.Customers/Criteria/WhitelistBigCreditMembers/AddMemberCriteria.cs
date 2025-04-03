using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.WhitelistBigCreditMembers
{
    public class AddMemberCriteria : CriteriaBase
    {
        public AddMemberCriteria() { }

        public AddMemberCriteria(string usernames, int userId, string ip)
            : this(usernames, 1, userId, ip)
        {
        }

        public AddMemberCriteria(string usernames, byte type, int userId, string ip)
        {
            Usernames = usernames;
            Type = type;
            UserId = userId;
            IP = ip;
        }

        public string Usernames { get; }

        public byte Type { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "Acc_VR_MemberBigStake_WhiteList_Upd_Manual";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Usernames) && UserId > 0;
    }
}
