using System.Collections.Generic;

namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class Policy
    {
        public Policy()
        {
        }

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

        public int PolicyId { get; set; }

        public string PolicyTitle { get; set; }

        public short SportType { get; set; }

        public Rule Rule { get; set; }

        public IEnumerable<Rule> Rules { get; set; }
    }
}
