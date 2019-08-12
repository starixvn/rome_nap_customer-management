using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class ReduceBLCriteria : InactivateCriteria
    {
        public ReduceBLCriteria(
            string custIds,
            byte roleId,
            int maxPerEvent,
            int maxPerBet,
            int userId,
            string ip) : base(custIds, roleId, userId, ip)
        {
            MaxPerEvent = maxPerEvent;
            MaxPerBet = maxPerBet;
        }

        public int MaxPerEvent { get; }

        [SpParam(SqlParamName = "maxbet")]
        public int MaxPerBet { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_ReduceBetLimit";

        public override bool IsValid()
            => base.IsValid() && MaxPerEvent > 0 && MaxPerBet > 0;
    }
}
