namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal class AddSpreadRuleResponse : Result
    {
        public AddSpreadRuleResponse(int policyId, int code, string message) : base(code, message)
        {
            PolicyID = policyId;
        }

        public int PolicyID { get; set; }
    }
}
