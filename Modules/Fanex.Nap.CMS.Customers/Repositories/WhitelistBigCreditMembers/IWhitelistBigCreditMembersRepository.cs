using System.Collections.Generic;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Models.WhitelistBigCreditMembers;

namespace Fanex.Nap.CMS.Customers.Repositories.WhitelistBigCreditMembers
{
    public interface IWhitelistBigCreditMembersRepository
    {
        IEnumerable<AddMemberResult> AddMembers(ICriteria criteria);
    }
}
