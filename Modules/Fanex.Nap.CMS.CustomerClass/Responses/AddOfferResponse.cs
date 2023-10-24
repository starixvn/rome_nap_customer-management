namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    internal class AddOfferResponse : Result
    {
        public AddOfferResponse(int policyId, int code, string message) : base(code, message)
        {
            PolicyId = policyId;
        }

        public int PolicyId { get; set; }
    }
}
