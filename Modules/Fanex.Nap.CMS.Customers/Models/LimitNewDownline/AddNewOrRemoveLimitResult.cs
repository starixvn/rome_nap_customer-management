using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Models.LimitNewDownline
{
    public class AddNewOrRemoveLimitResult : Result
    {
        public AddNewOrRemoveLimitResult(int code, string message, int countMember = 0)
        {
            Code = code;
            Message = message;
            CountMember = countMember;
        }

        public int CountMember { get; }
    }
}
