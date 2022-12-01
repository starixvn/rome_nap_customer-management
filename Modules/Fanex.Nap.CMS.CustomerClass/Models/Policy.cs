using System.Collections.Generic;

namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class Policy
    {
        public Policy(int policyId, string policyTitle, short sportType, Rule rule)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            SportType = sportType;
            Rule = rule;
        }

        public Policy(int policyId, string policyTitle, short sportType, IEnumerable<Rule> rules)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            SportType = sportType;
            Rules = rules;
        }

        public int PolicyId { get; }

        public string PolicyTitle { get; }

        public short SportType { get; }

        public Rule Rule { get; }

        public IEnumerable<Rule> Rules { get; }
    }
}
