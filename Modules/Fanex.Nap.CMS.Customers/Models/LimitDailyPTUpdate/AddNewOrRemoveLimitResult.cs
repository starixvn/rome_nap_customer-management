namespace Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class AddNewOrRemoveLimitResult : Result
    {
        public AddNewOrRemoveLimitResult(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
