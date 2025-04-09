namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    using Fanex.Data.Repository;

    public class GlobalSettingListCriteria : CriteriaBase
    {
        public GlobalSettingListCriteria(string username, short sportType, int exchangeId)
        {
            Username = username;
            SportType = sportType;
            ExchangeId = exchangeId;
        }

        public string Username { get; }

        public short SportType { get; }

        public int ExchangeId { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingSelect";

        public override bool IsValid()
            => true;
    }
}
