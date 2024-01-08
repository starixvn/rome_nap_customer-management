namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal class CheckPolicyResponse : Result
    {
        public CheckPolicyResponse(string policies, int code, string message) : base(code, message)
        {
            Policies = policies;
        }

        public string Policies { get; set; }
    }
}
