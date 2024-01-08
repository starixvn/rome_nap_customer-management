namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal class CheckRuleResponse : Result
    {
        public CheckRuleResponse(string rules, int code, string message) : base(code, message)
        {
            Rules = rules;
        }

        public string Rules { get; set; }
    }
}
