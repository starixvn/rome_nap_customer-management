namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class UpdateMultipleMaxTurnoverCriteria : CommandBaseCriteria
    {
        public string GlobalSetting { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverUpdateMultiple";

        public override bool IsValid()
            => true;
    }
}
