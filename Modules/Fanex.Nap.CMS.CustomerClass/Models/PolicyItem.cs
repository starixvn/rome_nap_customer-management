namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System.Collections.Generic;

    internal class PolicyItem
    {
        public PolicyItem(
            int policyId,
            string policyTitle,
            short sportType,
            IEnumerable<int> ruleIds)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            SportType = sportType;
            RuleIds = ruleIds;
        }

        public int PolicyId { get; }

        public string PolicyTitle { get; }

        public short SportType { get; }

        public IEnumerable<int> RuleIds { get; }
    }
}
