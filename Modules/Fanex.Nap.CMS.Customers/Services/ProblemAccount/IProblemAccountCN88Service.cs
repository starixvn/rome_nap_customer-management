namespace Fanex.Nap.CMS.Customers.Services.ProblemAccount
{
    using Infrastructure.Models;

    public interface IProblemAccountCN88Service : IProblemAccountService
    {
        Result UpdateInformingStatus(
            string usernames,
            byte roleId,
            byte informType,
            bool isApplyDownlines,
            int userId,
            string ip);
    }
}
