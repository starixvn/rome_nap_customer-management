using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Models.WhitelistBigCreditMembers;

namespace Fanex.Nap.CMS.Customers.Services.WhitelistBigCreditMembers
{
    public interface IWhitelistBigCreditMembersService
    {
        IEnumerable<AddMemberResult> AddMembers(string usernames, int userId, string ip);
    }
}
